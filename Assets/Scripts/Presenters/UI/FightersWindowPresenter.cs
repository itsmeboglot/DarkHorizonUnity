using System.Collections.Generic;
using System.Linq;
using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Interface;
using Core.View.ViewPool;
using Darkhorizon.Shared.Dto;
using Darkhorizon.Shared.Player.Dto;
using Entities.Card;
using Unity.Settings;
using UnityEngine;
using UseCases.Addressables;
using UseCases.Menu;
using Utils.Extensions;
using Utils.Logger;
using Views.Cards;
using Views.UI;

namespace Presenters.UI
{
    public class FightersWindowPresenter : Presenter
    {
        private class Controller : Controller<FightersWindowView>, ButtonClick.ISubscribed, CardButtonClick.ISubscribed
        {
            private readonly ProfileUseCase _profileUseCase;
            private readonly ViewPool _viewPool;
            private readonly GameResourcesUseCase _resourcesUseCase;
            private readonly CardInfoUseCase _cardInfoUseCase;
            private readonly DeckEditingUseCase _deckEditingUseCase;
            private List<CollectionCardView> _cardsCollection = new List<CollectionCardView>();

            private ProfileDto _profile;
            private bool _isDeckEditing;

            public Controller(FightersWindowView view, IWindowHandler windowHandler,
                ProfileUseCase profileUseCase, ViewPool viewPool, GameResourcesUseCase resourcesUseCase,
                CardInfoUseCase cardInfoUseCase, DeckEditingUseCase deckEditingUseCase) : base(view, windowHandler)
            {
                _profileUseCase = profileUseCase;
                _viewPool = viewPool;
                _resourcesUseCase = resourcesUseCase;
                _cardInfoUseCase = cardInfoUseCase;
                _deckEditingUseCase = deckEditingUseCase;
            }

            void ButtonClick.ISubscribed.OnEvent(ButtonType value)
            {
                switch (value)
                {
                    case ButtonType.Close:
                        _deckEditingUseCase.Cancel();
                        Close();
                        break;
                }
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                _isDeckEditing = (bool) openData;
                _profile = _profileUseCase.GetProfile();

                ClearCards();
                if (_cardsCollection.Count == 0)
                {
                    CreateCards();
                }
                else
                {
                    
                }

            }

            public override void OnClose()
            {
                //ClearCards();
            }

            private void CreateCards()
            {
                for (int i = 0; i < _profile.CharacterCards.Length; i++)
                {
                    var cardDto = _profile.CharacterCards[i];
                    //CustomLogger.Log(LogSource.Unity, $"CreateCards Id {cardDto.Id}");
                    var cardData = cardDto.CreateCardFromDto();
                    var cardViewData = new CardViewData()
                    {
                        Entity = cardData,
                        Sprite = _resourcesUseCase.GetCardSpriteByUrl(_cardInfoUseCase.GetCardUrlById(cardData.Id)),
                        StatViewData = CreateStatsData(cardDto.Stats)
                    };
                        
                    var cardView = _viewPool.Pop<CollectionCardView>(Const.Poolables.CollectionCard);
                    cardView.Initialize(cardViewData, _isDeckEditing, _deckEditingUseCase.SelectedCards.Contains(cardData.Id), 
                        ConcreteView.TopBorder.position.y, ConcreteView.BottomBorder.position.y);
                    
                    //ConcreteView.SetCardToGrid(cardView);
                    _cardsCollection.Add(cardView);
                }

                ConcreteView.SetCardViewsToGrid(_cardsCollection);
            }

            private void ClearCards()
            {
                if (_cardsCollection.Count == 0) return;

                foreach (var card in _cardsCollection)
                {
                    _viewPool.Push(card);
                }

                _cardsCollection.Clear();
            }
            
            List<CardStatData> CreateStatsData (StatDto[] statDtos)
            {
                var statData = new List<CardStatData>(statDtos.Length);
                for (var i = 0; i < statDtos.Length; i++)
                {
                    var tmpStatData = new CardStatData
                    {
                        StatType = (CardStatsType) statDtos[i].Type,
                        Value = statDtos[i].Level
                    };
                    statData.Add(tmpStatData);
                }
                return statData;
            }

            public void OnEvent(int cardId)
            {
                if(_isDeckEditing)
                {
                    
                    _deckEditingUseCase.ChooseCard(cardId);
                    Close();
                }
                else
                {
                    var clickedCard = _cardsCollection.Find(x => x.Id == cardId);
                    if (clickedCard != null)
                    {
                        //clickedCard.
                    }
                }
            }
        }

        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<ButtonClick>()
                .BindEvent<CardButtonClick>();
        }

        public class ButtonClick : EventHub<ButtonClick, ButtonType>
        {
        }
        
        public class CardButtonClick : EventHub<CardButtonClick, int> // card Id
        {
        }


        public enum ButtonType
        {
            Close,
            Sacrifice
        }
    }
}