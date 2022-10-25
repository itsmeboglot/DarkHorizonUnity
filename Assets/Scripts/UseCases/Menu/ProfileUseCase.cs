using System;
using Darkhorizon.Shared.Player.Dto;
using Darkhorizon.Shared.Player.Protocol.Events;
using Gateways.Interfaces;
using Zenject;

namespace UseCases.Menu
{
    public class ProfileUseCase : IInitializable, IDisposable
    {
        public event Action<ProfileDto> OnProfileUpdated;
        
        private readonly IProfileRepository _profileRepository;

        public ProfileUseCase(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        
        void IInitializable.Initialize()
        {
            _profileRepository.OnProfileUpdated += HandleUpdate;
        }

        void IDisposable.Dispose()
        {
            _profileRepository.OnProfileUpdated -= HandleUpdate;
        }

        public void LoadProfile(Action<ProfileReceivedEvent> onSucceed = null, Action<NeedRegistrationEvent> onFail = null)
        {
            _profileRepository.LoadProfile(onSucceed, onFail);
        }

        public ProfileDto GetProfile()
        {
            return _profileRepository.GetProfile();
        }

        private void HandleUpdate(ProfileDto profile)
        {
            OnProfileUpdated?.Invoke(profile);
        }
    }
}