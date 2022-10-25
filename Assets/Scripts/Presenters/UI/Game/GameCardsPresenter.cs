using System;
using System.Collections.Generic;
using System.Linq;
using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Interface;
using Core.View.ViewPool;
using Cysharp.Threading.Tasks;
using Darkhorizon.Shared.Dto;
using Darkhorizon.Shared.Party.Dtos;
using Darkhorizon.Shared.Party.Protocol.Events;
using Entities.Card;
using Gateways.Interfaces;
using UnityEngine;
using UseCases.Addressables;
using UseCases.Game;
using UseCases.Menu;
using Utils.Extensions;
using Views.Cards;
using Views.UI;
using Object = UnityEngine.Object;

namespace Presenters.Cards
{
    public class GameCardsPresenter : Presenter
    {
        private class Controller : Controller<CardsPanelView>, CardStatButtonClick.ISubscribed,
            StartTurnButtonClick.ISubscribed, CardButtonClick.ISubscribed
        {
            private readonly GameResourcesUseCase _gameResourcesUseCase;
            private readonly GetUserWalletCollectionUseCase _getUserWalletCollectionUseCase;
            private readonly GameCommandsUseCase _gameCommandsUseCase;
            private readonly GameStateMachineUseCase _stateMachineUseCase;

            private readonly Dictionary<CardView, List<StatView>> _cardStatsByView =
                new Dictionary<CardView, List<StatView>>();

            private readonly List<CardView> _cardViews = new List<CardView>();
            private readonly List<BoosterView> _boosterViews = new List<BoosterView>();
            private readonly IWindowHandler _windowHandler;
            private readonly ViewPool _viewPool;

            private int _opponentCardsLostCount;
            private int _lastSelectedCardId = -1;
            private CardStatsType? _lastSelectedStat;
            private CardStatsType _lastPlayedStat;

            private List<CardStatsType> _lockedStatsOnDraw =
                new List<CardStatsType>(Enum.GetNames(typeof(CardStatsType)).Length);

            private GameDto _gameDto;
            private GameState _currentState = GameState.Idle;

            public Controller(CardsPanelView view, IWindowHandler windowHandler, ViewPool viewPool,
                GameResourcesUseCase gameResourcesUseCase, GameCommandsUseCase gameCommandsUseCase,
                GameStateMachineUseCase stateMachineUseCase) : base(view, windowHandler)
            {
                _windowHandler = windowHandler;
                _viewPool = viewPool;
                _gameResourcesUseCase = gameResourcesUseCase;
                _gameCommandsUseCase = gameCommandsUseCase;
                _stateMachineUseCase = stateMachineUseCase;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                _gameDto = (GameDto) openData;
                CreateMyCards(_gameDto.Your.CharacterCards);

                _stateMachineUseCase.SubscribeOnStateMachine(OnStateChangedHandler);
                _gameCommandsUseCase.OnBoosterUsed += HandleBoosterUsed;
                ConcreteView.BlockCardsPanel();
            }

            public override void OnClose()
            {
                _gameCommandsUseCase.OnBoosterUsed -= HandleBoosterUsed;

                _stateMachineUseCase.UnsubscribeFromStateMachine(OnStateChangedHandler);
                foreach (var view in _cardViews)
                {
                    var statViews = _cardStatsByView[view];
                    var statsCount = statViews.Count;
                    for (int i = 0; i < statsCount; i++)
                    {
                        Object.Destroy(statViews[i].gameObject);
                        //_viewPool.Push(statViews[i]);
                    }
                }

                _cardStatsByView.Clear();
                _cardViews.ForEach(x => Object.Destroy(x.gameObject));
                _cardViews.Clear();
                _boosterViews.Clear();
            }

            public void OnEvent(int cardId, CardStatsType statType)
            {
                _lastSelectedCardId = cardId;
                _lastSelectedStat = statType;
            }

            // Start turn clicked
            public void OnEvent()
            {
                HandleStartTurnBtnClick();
            }

            // Card clicked
            public void OnEvent(int value)
            {
                _lastSelectedCardId = value;
                ConcreteView.DeselectAllStats();
            }

            private void HandleStartTurnBtnClick()
            {
                if (_lastSelectedCardId == -1 || _lastSelectedStat == null)
                    return;

                _lastPlayedStat = _lastSelectedStat.Value;
                ConcreteView.BlockCardStat(_lastSelectedCardId, _lastSelectedStat.Value);
                switch (_currentState)
                {
                    case GameState.Attack:
                        ConcreteView.BlockCardsPanel();
                        _stateMachineUseCase.SwitchToWaitingState();
                        _gameCommandsUseCase.SendMyAttack(_lastSelectedCardId, _lastSelectedStat.Value);
                        break;
                    case GameState.Defence:
                        ConcreteView.BlockCardsPanel();
                        _stateMachineUseCase.SwitchToWaitingState();
                        _gameCommandsUseCase.SendMyDefence(_lastSelectedCardId);
                        break;
                    case GameState.SelectStat:
                        ConcreteView.UnBlockAllStats();
                        _gameCommandsUseCase.SendSelectedStat(_lastSelectedStat.Value);
                        break;
                }
            }

            private void OnStateChangedHandler(GameState state, object data)
            {
                _currentState = state;
                switch (state)
                {
                    case GameState.Idle:
                        ConcreteView.BlockCardsPanel();
                        break;
                    case GameState.WaitForOpponentTurn:
                        ConcreteView.BlockCardsPanel();
                        ConcreteView.ShowCardFaces();
                        break;
                    case GameState.BattleStarted:
                        ConcreteView.BlockCardsPanel();
                        ConcreteView.BattleStartCardsAnim(HandleBattleStarted);
                        break;
                    case GameState.Attack:
                        ConcreteView.UnBlockCards();
                        ConcreteView.DeBoostCards();
                        ConcreteView.ShowGameBoard(true);
                        ConcreteView.ShowCardStats();
                        ConcreteView.UnblockCardsPanel();
                        ConcreteView.UnBlockAllStats();
                        ConcreteView.ShowCardSelectBtns();
                        _lockedStatsOnDraw.Clear();
                        break;
                    case GameState.Defence:
                        ConcreteView.UnBlockCards();
                        ConcreteView.DeBoostCards();
                        ConcreteView.ShowGameBoard(true);
                        ConcreteView.ShowCardStats();
                        ConcreteView.UnblockCardsPanel();
                        var defendData = (YourDefendEvent) data;
                        _lastPlayedStat = (CardStatsType) defendData.AttackerStatType; // save stat for auto defend
                        ConcreteView.BlockAllStatsExcept((CardStatsType) defendData.AttackerStatType);
                        ConcreteView.ShowCardSelectBtns();
                        _lockedStatsOnDraw.Clear();
                        break;
                    case GameState.SelectStat:
                        _lockedStatsOnDraw.Add(_lastPlayedStat);
                        ConcreteView.UnBlockCards();
                        ConcreteView.DeBoostCards();
                        ConcreteView.ShowGameBoard(true);
                        ConcreteView.UnBlockAllStats();
                        ConcreteView.ShowCardStats(_lastSelectedCardId, _lockedStatsOnDraw.ToArray());
                        ConcreteView.UnblockCardsPanel();
                        //ConcreteView.BlockCardsExcept(_lastSelectedCardId);
                        ConcreteView.ShowCardSelectBtns();
                        break;
                    case GameState.EndOfTurn:
                        HandleEndOfTurn(data);
                        break;
                    case GameState.BattleFinished:
                        _opponentCardsLostCount = 0;
                        break;

                    // Auto fight when time is up.
                    case GameState.AutoAttack:
                    {
                        _lastSelectedCardId = ((YourAutoAttackEvent) data).CardId;
                        _lastPlayedStat = (CardStatsType) ((YourAutoAttackEvent) data).StatType;
                        ConcreteView.BlockCardsPanel();
                        ConcreteView.BlockCardStat(_lastSelectedCardId, _lastPlayedStat);
                        _stateMachineUseCase.SwitchToWaitingState();

                        break;
                    }
                    case GameState.AutoDefend:
                    {
                        _lastSelectedCardId = ((YourAutoDefendEvent) data).CardId;
                        ConcreteView.BlockCardsPanel();
                        ConcreteView.BlockCardStat(_lastSelectedCardId, _lastPlayedStat);
                        _stateMachineUseCase.SwitchToWaitingState();
                        break;
                    }
                    case GameState.AutoSelectStat:
                        _lastPlayedStat = (CardStatsType) ((YourAutoSelectStatEvent) data).StatType;
                        ConcreteView.BlockCardStat(_lastSelectedCardId, _lastPlayedStat);
                        break;
                    case GameState.OpponentReplenishTime:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(state), state, null);
                }
            }

            private void CreateMyCards(CharacterCardDto[] cardDtos)
            {
                for (int i = 0; i < cardDtos.Length; i++)
                {
                    var cardView = Object.Instantiate(ConcreteView.CardPrefab, ConcreteView.CardCardContainer);
                    var dto = cardDtos[i];
                    var sprite = _gameResourcesUseCase.GetCardSpriteByUrl(dto.ImageUrl);
                    var cardEntity = dto.CreateCardFromDto();
                    var data = new CardViewData
                    {
                        Entity = cardEntity,
                        Sprite = sprite
                    };
                    var statsContainer = CreateStatsViews(dto.Stats, cardView.CardStatsContainer);
                    data.StatViewData = statsContainer.statDatas;
                    cardView.SetData(data, statsContainer.statViews);
                    cardView.ShowCardFace();

                    _cardViews.Add(cardView);
                    _cardStatsByView.Add(cardView, statsContainer.statViews);
                }

                (List<StatView> statViews, List<CardStatData> statDatas) CreateStatsViews(StatDto[] statDtos,
                    Transform statsContainer)
                {
                    var views = new List<StatView>(statDtos.Length);
                    var statData = new List<CardStatData>(statDtos.Length);
                    for (var i = 0; i < statDtos.Length; i++)
                    {
                        var statView =
                            Object.Instantiate(ConcreteView.StatPrefab,
                                statsContainer); //_viewPool.Pop<StatView>(Const.Poolables.CardStat, statsContainer);
                        var tmpStatData = new CardStatData
                        {
                            StatType = (CardStatsType) statDtos[i].Type,
                            Value = statDtos[i].Level
                        };
                        statView.SetData(tmpStatData);
                        views.Add(statView);
                        statData.Add(tmpStatData);
                    }

                    return (views, statData);
                }
            }

            private void HandleBattleStarted()
            {
                ConcreteView.InitializeCardsAnimPreparations(_cardViews);

                ConcreteView.UnBlockAllStats();
                ConcreteView.UnblockCardsPanel();
                ConcreteView.FlipCards();
            }

            private async void HandleEndOfTurn(object data)
            {
                var endOfTurnData = (RoundEndEvent) data;

                ConcreteView.ShowGameBoard(false);
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                var myLose = endOfTurnData.Result == RoundResultDto.Lose;

                if (myLose)
                {
                    ConcreteView.UseCard(endOfTurnData.YourCardId);
                }
                else
                {
                    _opponentCardsLostCount++;
                    ConcreteView.LoseOpponentsCard();
                }
            }

            private void HandleBoosterUsed(int boosterId)
            {
                if (_lastSelectedCardId == -1)
                {
                    // ToDo: info window
                    return;
                }

                var boosterData =
                    _gameDto.Your.BoosterCards.FirstOrDefault(x => x.Id == boosterId) as StatBoosterCardDto;
                var firstEffect = boosterData.Effects[0];
                if (firstEffect.Target == BoosterTargetDto.Self)
                    ConcreteView.UseBooster(_lastSelectedCardId, (CardStatsType) firstEffect.StatType,
                        firstEffect.Value);
            }
        }

        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<CardStatButtonClick>()
                .BindEvent<StartTurnButtonClick>();
        }

        public class CardStatButtonClick : EventHub<CardStatButtonClick, int, CardStatsType>
        {
        }

        public class CardButtonClick : EventHub<CardButtonClick, int>
        {
        }

        public class StartTurnButtonClick : EventHub<StartTurnButtonClick>
        {
        }

        public enum ButtonType
        {
            StartTurn,
            Card
        }
    }
}