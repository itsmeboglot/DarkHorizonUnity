using System;
using Darkhorizon.Shared.Player.Dto;
using Darkhorizon.Shared.Player.Protocol.Events;
using Gateways.Interfaces;

namespace Gateways
{
    public sealed class ProfileRepository : IProfileRepository, IDisposable
    {
        public event Action<ProfileDto> OnProfileUpdated;

        private readonly IHttpApiGateway _httpApiGateway;
        private ProfileDto _profile;

        public ProfileRepository(IHttpApiGateway httpApiGateway)
        {
            _httpApiGateway = httpApiGateway;
            _httpApiGateway.EventHub.Subscribe<ProfileReceivedEvent>(HandleProfileUpdate);
        }

        void IDisposable.Dispose()
        {
            _httpApiGateway.EventHub.Unsubscribe<ProfileReceivedEvent>(HandleProfileUpdate);
        }


        public ProfileDto GetProfile()
        {
            return _profile;
        }

        public void LoadProfile(Action<ProfileReceivedEvent> onSucceed, Action<NeedRegistrationEvent> onFail = null)
        {
            _httpApiGateway.EventHub.Subscribe<ProfileReceivedEvent>(HandleSucceed);
            _httpApiGateway.EventHub.Subscribe<NeedRegistrationEvent>(HandleFail);
            _httpApiGateway.ApiHub.Player.GetProfile();

            void HandleSucceed(ProfileReceivedEvent @event)
            {
                _httpApiGateway.EventHub.Unsubscribe<ProfileReceivedEvent>(HandleSucceed);
                _httpApiGateway.EventHub.Unsubscribe<NeedRegistrationEvent>(HandleFail);
                onSucceed?.Invoke(@event);
            }

            void HandleFail(NeedRegistrationEvent @event)
            {
                _httpApiGateway.EventHub.Unsubscribe<ProfileReceivedEvent>(HandleSucceed);
                _httpApiGateway.EventHub.Unsubscribe<NeedRegistrationEvent>(HandleFail);
                onFail?.Invoke(@event);
            }
        }
        
        private void HandleProfileUpdate(ProfileReceivedEvent @event)
        {
            _profile = @event.Profile;
            OnProfileUpdated?.Invoke(@event.Profile);
        }
    }
}