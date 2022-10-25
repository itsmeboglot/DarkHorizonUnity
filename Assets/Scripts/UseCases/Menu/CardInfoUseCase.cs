using System;
using System.Collections.Concurrent;
using Darkhorizon.Shared.Player.Dto;
using Gateways.Interfaces;
using Zenject;

namespace UseCases.Menu
{
    public class CardInfoUseCase : IInitializable, IDisposable
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ConcurrentDictionary<int, string> _cardsSelection = new ConcurrentDictionary<int, string>();
        
        private ProfileDto _profile;

        public CardInfoUseCase(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        
        void IInitializable.Initialize()
        {
            _profile = _profileRepository.GetProfile();
            
            if(_profile != null)
                SampleCards();

            _profileRepository.OnProfileUpdated += HandleUpdate;
        }
        
        void IDisposable.Dispose()
        {
            _profileRepository.OnProfileUpdated -= HandleUpdate;
        }

        public string GetCardUrlById(int cardId)
        {
            return _cardsSelection.GetOrAdd(cardId, string.Empty);
        }

        private void SampleCards()
        {
            _cardsSelection.Clear();
            
            foreach (var cardDto in _profile.CharacterCards)
            {
                _cardsSelection.TryAdd(cardDto.Id, cardDto.ImageUrl);
            }
        }

        private void HandleUpdate(ProfileDto profile)
        {
            _profile = profile;
            SampleCards();
        }
    }
}