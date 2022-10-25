using System;
using Darkhorizon.Shared.Player.Protocol.Events;
using Gateways.Interfaces;
using Zenject;

namespace UseCases.Common
{
    public class NotificationListenerUseCase : IInitializable, IDisposable
    {
        public event Action<string> OnNotification;

        private readonly IHttpApiGateway _httpApiGateway;

        public NotificationListenerUseCase(IHttpApiGateway httpApiGateway)
        {
            _httpApiGateway = httpApiGateway;
        }

        public void Initialize()
        {
            _httpApiGateway.EventHub.Subscribe<NickNameAlreadyExistsEvent>(HandleNickNameNotification);
        }

        public void Dispose()
        {
            _httpApiGateway.EventHub.Unsubscribe<NickNameAlreadyExistsEvent>(HandleNickNameNotification);
        }

        private void HandleNickNameNotification(NickNameAlreadyExistsEvent notification)
        {
            OnNotification?.Invoke("This nickname is already used by someone.");
        }
    }
}