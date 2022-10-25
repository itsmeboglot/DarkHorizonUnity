using System;
using Gateways.Interfaces;
using Utils.Logger;
using Whimsy.Shared.Core.Protocol.Events;

namespace UseCases.Common
{
    public class ErrorListenerUseCase : IDisposable
    {
        private readonly IHttpApiGateway _httpApiGateway;
        private readonly ILobbyGateway _lobbyGateway;

        public ErrorListenerUseCase(IHttpApiGateway httpApiGateway, ILobbyGateway lobbyGateway)
        {
            _httpApiGateway = httpApiGateway;
            _lobbyGateway = lobbyGateway;
            
            _lobbyGateway.EventHub.Subscribe<ErrorEvent>(HandleErrors);
            _httpApiGateway.EventHub.Subscribe<ErrorEvent>(HandleErrors);
        }
        
        void IDisposable.Dispose()
        {
            _lobbyGateway.EventHub.Unsubscribe<ErrorEvent>(HandleErrors);
            _httpApiGateway.EventHub.Unsubscribe<ErrorEvent>(HandleErrors);
        }

        public void SubscribeLobbyHandling()
        {
            _lobbyGateway.EventHub.Subscribe<ErrorEvent>(HandleErrors);
        }

        private void HandleErrors(ErrorEvent errorEvent)
        {
            CustomLogger.Log(LogSource.Server, $"ERROR EBU4ii {errorEvent.Message}", MessageType.Error);
        }
    }
}