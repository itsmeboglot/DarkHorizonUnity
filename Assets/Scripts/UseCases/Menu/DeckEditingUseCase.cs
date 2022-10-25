using System;
using System.Collections.Generic;
using Darkhorizon.Shared.Player.Dto;
using Darkhorizon.Shared.Player.Protocol.Events;
using Gateways.Interfaces;
using Utils.Logger;

namespace UseCases.Menu
{
    public class DeckEditingUseCase
    {
        public event Action<DeckDto> OnDeckChanged;
        public event Action<int> OnCardSelect;
        public event Action<int> OnCardDeselect;
        public event Action OnCancel;
        
        private readonly IHttpApiGateway _httpApiGateway;
        private readonly List<int> _selectedCards = new List<int>();

        public IReadOnlyList<int> SelectedCards => _selectedCards;

        public DeckEditingUseCase(IHttpApiGateway httpApiGateway)
        {
            _httpApiGateway = httpApiGateway;
        }

        public void Cancel()
        {
            OnCancel?.Invoke();
        }
        
        public void ChooseCard(int id)
        {
            OnCardSelect?.Invoke(id);
            _selectedCards.Add(id);
        }

        public void DeselectCard(int id)
        {
            OnCardDeselect?.Invoke(id);
            _selectedCards.Remove(id);
        }

        public void SaveDeck(int[] cardIds, int[] boosterIds, int deckId)
        {
            var deck = new DeckDto()
            {
                Id = deckId,
                BoosterCardsIds = boosterIds,
                CharacterCardsIds = cardIds
            };
        }
        
        public void SaveDeck(DeckDto deckDto)
        {
            _httpApiGateway.EventHub.Subscribe<EditDeckSucceedEvent>(DeckEditingSucceed);
            _httpApiGateway.ApiHub.Player.EditDeck(deckDto);
        }

        private void DeckEditingSucceed(EditDeckSucceedEvent @event)
        {
            OnDeckChanged?.Invoke(@event.EditedDeckDto);
        }
    }
}