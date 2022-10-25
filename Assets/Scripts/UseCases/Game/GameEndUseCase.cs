using System;
using Darkhorizon.Shared.Party.Protocol.Events;
using Gateways.Interfaces;
using UseCases.Common;

namespace UseCases.Game
{
    public class GameEndUseCase
    {
        private readonly ILobbyGateway _lobbyGateway;
        private readonly ErrorListenerUseCase _errorListenerUseCase;

        public GameEndUseCase(ILobbyGateway lobbyGateway, ErrorListenerUseCase errorListenerUseCase)
        {
            _lobbyGateway = lobbyGateway;
            _errorListenerUseCase = errorListenerUseCase;
        }

        public void SubscribeOnPartyFinish(Action<PartyFinishedEvent> doNext)
        {
            _lobbyGateway.EventHub.Subscribe(doNext);
        }
        
        public void UnsubscribeOnPartyFinish(Action<PartyFinishedEvent> doNext)
        {
            _lobbyGateway.EventHub.Unsubscribe(doNext);
        }

        public void CloseConnection()
        {
            //_lobbyGateway.ClearSubscribes();
            _lobbyGateway.Disconnect();
            _errorListenerUseCase.SubscribeLobbyHandling();
        }
    }
}