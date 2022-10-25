using System;
using System.Collections.Generic;
using System.Linq;
using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using Cysharp.Threading.Tasks;
using Darkhorizon.Shared.Lobby.Protocol.Events;
using Darkhorizon.Shared.Party.Dtos;
using Darkhorizon.Shared.Party.Protocol.Events;
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
using SharedDto = Darkhorizon.Shared.Dto;

namespace Presenters.UI
{
    public class PreGamePresenter : Presenter
    {
        private class Controller : Controller<PreGamePanelView>, ButtonClick.ISubscribed, CardButtonClick.ISubscribed
        {
            private readonly IWindowHandler _windowHandler;
            private readonly LobbyConnectionUseCase _lobbyConnectionUseCase;
            private readonly SearchGameUseCase _searchGameUseCase;
            private readonly ProfileUseCase _profileUseCase;
            private readonly GameResourcesUseCase _gameResourcesUseCase;
            private readonly DeckEditingUseCase _deckEditingUseCase;
            private ProfileDto _profileDto;
            private int _lastSelectedCardIndex = -1;
            private List<DeckCardView> _cardsInDeck;
            private int _lastSelectedCardId;
            private DeckDto _deckDto;

            public Controller(PreGamePanelView view, IWindowHandler windowHandler, LobbyConnectionUseCase lobbyConnectionUseCase,
                SearchGameUseCase searchGameUseCase, ProfileUseCase profileUseCase, GameResourcesUseCase gameResourcesUseCase, 
                DeckEditingUseCase deckEditingUseCase) 
                : base(view, windowHandler)
            {
                _windowHandler = windowHandler;
                _lobbyConnectionUseCase = lobbyConnectionUseCase;
                _searchGameUseCase = searchGameUseCase;
                _profileUseCase = profileUseCase;
                _gameResourcesUseCase = gameResourcesUseCase;
                _deckEditingUseCase = deckEditingUseCase;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                _profileDto = _profileUseCase.GetProfile();
                _deckEditingUseCase.OnCardSelect += HandleCardSelect;
                _deckEditingUseCase.OnCancel += HandleCancel;
                
                ConcreteView.EnableCloseButton();
                // ToDo: remove when implements multi decks
                if (_profileDto.Decks.Length > 0)
                {
                    _deckDto = _profileDto.Decks[0];
                    ConcreteView.EnablePlayButton();
                }
                else
                {
                    ConcreteView.DisablePlayButton();
                    var randomBoostersIds = _profileDto.BoosterCards.Select(x => x.Id).ToArray();
                    var randomCardIds = _profileDto.CharacterCards.Select(x => x.Id).ToArray();
                    var randomBoosterIndex = UnityEngine.Random.Range(0, randomBoostersIds.Length - Const.GameValues.MaxBoostersCount);
                    var randomCardIndex = UnityEngine.Random.Range(0, randomCardIds.Length - Const.GameValues.MaxCardsCount);
                    _deckDto = new DeckDto
                    {
                        Id = 0,
                        BoosterCardsIds = new []
                        {
                            randomBoostersIds[randomBoosterIndex++],
                            randomBoostersIds[randomBoosterIndex]
                        },
                        CharacterCardsIds = new int [Const.GameValues.MaxCardsCount]
                        {
                            randomCardIds[randomCardIndex++], 
                            randomCardIds[randomCardIndex++], 
                            randomCardIds[randomCardIndex++], 
                            randomCardIds[randomCardIndex++], 
                            randomCardIds[randomCardIndex]
                        }
                    };
                }

                var viewData = CreateCardViewDataByIds (_deckDto.CharacterCardsIds);
                ConcreteView.InitWithData(viewData);
            }

            private void HandleCancel()
            {
                ConcreteView.Show();
            }

            public override void OnClose()
            {
                ConcreteView.EnablePlayButton();
                _deckEditingUseCase.OnCardSelect -= HandleCardSelect;
                _deckEditingUseCase.OnCancel -= HandleCancel;
            }
                        
            private void HandleCardSelect(int id)
            {
                ConcreteView.Show();

                if (_lastSelectedCardId != -1)
                {
                    _deckEditingUseCase.DeselectCard(_lastSelectedCardId);
                }

                
                _deckDto.CharacterCardsIds[_lastSelectedCardIndex] = id;
                var data = CreateCardViewDataById(id);
                ConcreteView.UpdateCardByIndex(_lastSelectedCardIndex, data);
                _lastSelectedCardIndex = -1;
            }

            void ButtonClick.ISubscribed.OnEvent(ButtonType buttonType)
            {
                switch (buttonType)
                {
                    case ButtonType.Close:
                        // CancelSearch();
                        
                        Close();
                        break;
                    case ButtonType.Play:
                        ConcreteView.DisableCloseBtn();
                        Play();
                        break;
                    case ButtonType.SaveDeck:
                        FillEmptyDeckIfNeeds();
                        _deckEditingUseCase.OnDeckChanged += HandleDeckChanged;
                        ConcreteView.DisablePlayButton();
                        _deckEditingUseCase.SaveDeck(_deckDto);
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
                }
            }

            private void HandleDeckChanged(DeckDto dto)
            {
                ConcreteView.EnablePlayButton();
                _deckEditingUseCase.OnDeckChanged -= HandleDeckChanged;
                
                _deckDto = dto;
            }

            void CardButtonClick.ISubscribed.OnEvent(int cardIndex, int cardId)
            {
                ConcreteView.Hide();
                _lastSelectedCardIndex = cardIndex;
                _lastSelectedCardId = cardId;
                
                _windowHandler.OpenWindow(WindowType.FightersWindow, true);
            }

            private async void Play()
            {
                ConcreteView.DisablePlayButton();
                var isConnected = await _lobbyConnectionUseCase.Connect();
                if (!isConnected)
                {
                    ConcreteView.DisablePlayButton();
                    return;
                }
                
                _lobbyConnectionUseCase.JoinLobby(HandleLobbyJoined);
            }

            private void HandleLobbyJoined(EnterLobbySucceedEvent @event)
            {
                _searchGameUseCase.StartMatchmaking(HandlePartyInvite);
            }
            
            private void HandlePartyInvite(PartyInviteReceivedEvent @event)
            {
                _searchGameUseCase.JoinParty(@event.ServerUrl, @event.Ticket, HandleMatchReady);
            }

            private async void HandleMatchReady(JoinPartySucceedEvent @event)
            {
                //_lobbyConnectionUseCase.ClearLobbySubscribes();
                await UniTask.WaitUntil(() => _profileDto != null);
                
                _gameResourcesUseCase.LoadCardSprites(new [] {@event.Game.Other.AvatarImageUrl}, () =>
                {
                    StartBattle(@event.Game);
                });
            }

            private async void StartBattle(GameDto gameDto)
            {
                ConcreteView.EnablePlayButton();
                Close();
                WindowHandler.OpenWindow(WindowType.MatchFound, gameDto);
                
                await UniTask.Delay(TimeSpan.FromSeconds(2));
                
                WindowHandler.OpenWindow(WindowType.GameLoad, gameDto);
            }

            private void CancelSearch()
            {
                _lobbyConnectionUseCase.ClearLobbySubscribes();
                _lobbyConnectionUseCase.Disconnect();
            }

            private void FillEmptyDeckIfNeeds()
            {
                for (var i = 0; i < ConcreteView.CurrentCardIds.Count; i++)
                {
                    if (ConcreteView.CurrentCardIds[i] == -1)
                    {
                        var usedIds = ConcreteView.CurrentCardIds;
                        var id = _profileDto.CharacterCards.GetExclusiveId(usedIds);
                        _deckDto.CharacterCardsIds[i] = id;
                        ConcreteView.UpdateCardByIndex(i, CreateCardViewDataById(id));
                    }
                }
            }
            
            private List<CardViewData> CreateCardViewDataByIds(int[] ids)
            {
                var viewData = new List<CardViewData>();

                if (ids.Length == 0)
                {
                    ids = new int[Const.GameValues.MaxCardsCount];
                    for (int i = 0; i < Const.GameValues.MaxCardsCount; i++)
                    {
                        ids[i] = -1;
                    }
                }
                
                foreach (var id in ids)
                {
                    viewData.Add(CreateCardViewDataById(id));
                }

                return viewData;
            }

            private CardViewData CreateCardViewDataById (int id)
            {
                if (id == -1)
                    return null;
                
                var cardDto = _profileDto.CharacterCards.FirstOrDefault(x => x.Id == id);
                var spriteUrl = cardDto.ImageUrl;
   
                var cardData = new CardViewData
                {
                    Sprite = _gameResourcesUseCase.GetCardSpriteByUrl(spriteUrl),
                    Entity = cardDto.CreateCardFromDto(),
                    StatViewData = CreateCardStatData(cardDto.Stats)
                };
                
                List<CardStatData> CreateCardStatData (SharedDto.StatDto[] statDtos)
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
                return cardData;
            }
        }
        
        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<ButtonClick>()
                .BindEvent<CardButtonClick>();
        }

        public class CardButtonClick : EventHub<CardButtonClick, int, int>
        {
            
        }
        
        public class ButtonClick : EventHub<ButtonClick, ButtonType>
        {
            
        }

        public enum ButtonType
        {
            Close,
            Play,
            SaveDeck
        }
    }
}