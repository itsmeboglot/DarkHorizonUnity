using System;
using Darkhorizon.Client.ApiHub;
using Gateways.Connection;
using Gateways.EventHub;
using Gateways.Interfaces;
using Utils;
using Whimsy.Shared.Core;
using Whimsy.Shared.Identity;
using Zenject;

namespace Gateways
{
    public sealed class HttpApiGateway : IHttpApiGateway, IInitializable, IDisposable
    {
        private readonly ServerEventHub _serverEventHub;
        private HttpTransport _identityTransport;
        private HttpTransport _playerTransport;
        
        public UserToken CurrentUser { get; private set; }
        public IDarkhorizonServerApiHub ApiHub { get; }
        public IWalletCollectionServerApi WalletApi { get; }
        public IServerEventHub EventHub => _serverEventHub;
        public void SetCurrentUser(UserToken token) => CurrentUser = token;

        public HttpApiGateway(ServerUris.Http uris)
        {
            ApiHub = BuildApiHub(uris);
            _serverEventHub = new ServerEventHub();
        }
        
        public void Initialize()
        {
            ApiHub.Events.OnEvent += HandleServerEvents;
        }

        public void Dispose()
        {
            ApiHub.Events.OnEvent -= HandleServerEvents;
        }

        private IDarkhorizonServerApiHub BuildApiHub(ServerUris.Http uris)
        {
            var serializer = new ServerSerializer();
            
            _identityTransport = new HttpTransport(uris.identityUri, 20, serializer);
            _playerTransport = new HttpTransport(uris.playerUri, 20, serializer);
            
            var configuration = new DarkhorizonServerApiHubConfiguration()
            {
                Serializer = serializer,
                IdentityTransport = _identityTransport,
                PlayerTransport = _playerTransport
            };

            return configuration.Build();
        }

        private void HandleServerEvents(IResponseEvent responseEvent)
        {
            _serverEventHub.Notify(responseEvent);
        }
    }
}