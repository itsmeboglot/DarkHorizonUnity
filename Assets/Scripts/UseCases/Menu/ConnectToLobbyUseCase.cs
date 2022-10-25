using System;
using Cysharp.Threading.Tasks;
using Darkhorizon.Shared.Lobby.Protocol.Commands;
using Darkhorizon.Shared.Lobby.Protocol.Events;
using Gateways.Interfaces;
using UseCases.Common;
using Utils.Logger;

namespace UseCases.Menu
{
    public class LobbyConnectionUseCase
    {
        
        
        private readonly ILobbyGateway _lobbyGateway;
        private readonly IHttpApiGateway _httpApiGateway;
        private readonly ErrorListenerUseCase _errorListenerUseCase;

        public LobbyConnectionUseCase(ILobbyGateway lobbyGateway, IHttpApiGateway httpApiGateway,
            ErrorListenerUseCase errorListenerUseCase)
        {
            _lobbyGateway = lobbyGateway;
            _httpApiGateway = httpApiGateway;
            _errorListenerUseCase = errorListenerUseCase;
        }

        public async UniTask<bool> Connect()
        {
            _lobbyGateway.OnSocketOpened += LobbyGatewayOnSocketOpened;
            _lobbyGateway.Connect();
            await UniTask.WaitUntil(() => _lobbyGateway.IsConnected);
            
            return true;
        }

        private void LobbyGatewayOnSocketOpened()
        {
            _lobbyGateway.Send(new JoinLobbyCommand {UserToken = _httpApiGateway.CurrentUser});
        }

        public void Disconnect()
        {
            _lobbyGateway.OnSocketOpened -= LobbyGatewayOnSocketOpened;

            if (_lobbyGateway.IsConnected)
                _lobbyGateway.Disconnect();
        }

        public async void JoinLobby(Action<EnterLobbySucceedEvent> onSucceed = null)
        {
            await UniTask.WaitUntil(() => _lobbyGateway.IsConnected);
            _lobbyGateway.EventHub.Subscribe<EnterLobbySucceedEvent>(HandleSucceed);

            void HandleSucceed(EnterLobbySucceedEvent @event)
            {
                _lobbyGateway.EventHub.Unsubscribe<EnterLobbySucceedEvent>(HandleSucceed);
                onSucceed?.Invoke(@event);
            }
        }

        public void ClearLobbySubscribes()
        {
            _lobbyGateway.ClearSubscribes();
            _errorListenerUseCase.SubscribeLobbyHandling();
        }
    }
}