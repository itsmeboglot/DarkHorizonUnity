using System;
using Darkhorizon.Shared.Player.Dto;
using Darkhorizon.Shared.Player.Protocol.Events;
using Gateways.Interfaces;

namespace UseCases.Menu
{
    public class RegisterUserUseCase
    {
        private readonly IHttpApiGateway _httpApiGateway;

        public RegisterUserUseCase(IHttpApiGateway httpApiGateway)
        {
            _httpApiGateway = httpApiGateway;
        }

        public void Register(string nickname, string avatarId, string bio,
            Action<RegisterSucceedEvent> onSucceed = null, Action<ProfileReceivedEvent> onProfile = null)
        {
            _httpApiGateway.EventHub.Subscribe<RegisterSucceedEvent>(HandleSucceed);
            _httpApiGateway.EventHub.Subscribe<ProfileReceivedEvent>(HandleProfile);

            _httpApiGateway.ApiHub.Player.Register(new PersonalInfoDto
            {
                NickName = nickname,
                Avatar = avatarId,
                Bio = bio
            });

            void HandleSucceed(RegisterSucceedEvent @event)
            {
                _httpApiGateway.EventHub.Unsubscribe<RegisterSucceedEvent>(HandleSucceed);
                onSucceed?.Invoke(@event);
            }
            
            void HandleProfile(ProfileReceivedEvent @event)
            {
                _httpApiGateway.EventHub.Unsubscribe<ProfileReceivedEvent>(HandleProfile);
                onProfile?.Invoke(@event);
            }
        }
    }
}