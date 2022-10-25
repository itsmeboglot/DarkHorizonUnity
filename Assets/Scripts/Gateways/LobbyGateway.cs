using System;
using Utils.Extensions;

namespace Gateways
{
    using Interfaces;

    public sealed class LobbyGateway : BaseSocketGateway, ILobbyGateway
    {
        public event Action OnSocketOpened;

        private readonly string _serverUri;

        public LobbyGateway(string serverUri) : base(serverUri)
        {
            _serverUri = serverUri;
        }

        public string GetPort()
        {
            return _serverUri.GetLastUntilOrEmpty(":");
        }
        
        protected override void HandleConnectionOpened()
        {
            OnSocketOpened?.Invoke();
        }
    }
}