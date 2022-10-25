using System;
using Darkhorizon.Shared.Player.Dto;
using Darkhorizon.Shared.Player.Protocol.Events;
using Gateways.Interfaces;

namespace UseCases.Menu
{
    public class GetUserWalletCollectionUseCase
    {
        private readonly IHttpApiGateway _httpApiGateway;

        public GetUserWalletCollectionUseCase(IHttpApiGateway httpApiGateway)
        {
            _httpApiGateway = httpApiGateway;
        }

        public void GetWalletCollection()
        {
            _httpApiGateway.WalletApi.GetWalletCollection();
        }
        
        public void SubscribeProfileReceived(Action<WalletCollectionReceivedEvent> doNext)
        {
            _httpApiGateway.EventHub.Subscribe(doNext);
        }
        
        public void UnsubscribeProfileReceived(Action<WalletCollectionReceivedEvent> doNext)
        {
            _httpApiGateway.EventHub.Unsubscribe(doNext);
        }
    }
}