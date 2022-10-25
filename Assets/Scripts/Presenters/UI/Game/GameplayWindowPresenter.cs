using System;
using System.Collections.Generic;
using System.Linq;
using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using Cysharp.Threading.Tasks;
using Darkhorizon.Shared.Dto;
using Darkhorizon.Shared.Party.Dtos;
using Darkhorizon.Shared.Party.Protocol.Events;
using Darkhorizon.Shared.Player.Dto;
using Entities.Card;
using Game.Audio;
using Gateways.Interfaces;
using UnityEngine;
using UseCases.Addressables;
using UseCases.Game;
using UseCases.Menu;
using Utils.Extensions;
using Utils.Logger;
using Views.UI;
using Views.WithoutPresenter;

namespace Presenters.UI
{
    public class GameplayWindowPresenter : Presenter
    {
        private class Controller : Controller<GameplayView>, ButtonClick.ISubscribed
        {
            private readonly IWindowHandler _windowHandler;
            private readonly GameplayUseCase _gameplayUseCase;
            private readonly GameResourcesUseCase _gameResourcesUseCase;
            private readonly GameEndUseCase _gameEndUseCase;
            private readonly IAudioPlayer _audioPlayer;
            private readonly GameStateMachineUseCase _gameStateMachine;
            private readonly ProfileUseCase _profileUseCase;
            private readonly GameCommandsUseCase _gameCommandsUseCase;
            private GameDto _gameDto;
            private ProfileDto _profileDto;
            private readonly List<bool> _winLoses = new List<bool>();
            private Sprite _myAvatarSprite;
            private Sprite _opponentAvatarSprite;

            public Controller(GameplayView view, IWindowHandler windowHandler,
                GameplayUseCase gameplayUseCase, GameResourcesUseCase gameResourcesUseCase,
                GameEndUseCase gameEndUseCase, IAudioPlayer audioPlayer, GameStateMachineUseCase gameStateMachine,
                ProfileUseCase profileUseCase, GameCommandsUseCase gameCommandsUseCase)
                : base(view, windowHandler)
            {
                _windowHandler = windowHandler;
                _gameplayUseCase = gameplayUseCase;
                _gameResourcesUseCase = gameResourcesUseCase;
                _gameEndUseCase = gameEndUseCase;
                _audioPlayer = audioPlayer;
                _gameStateMachine = gameStateMachine;
                _profileUseCase = profileUseCase;
                _gameCommandsUseCase = gameCommandsUseCase;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                _gameStateMachine.StartGameStateMachine();
                _gameStateMachine.SubscribeOnStateMachine(OnStateChanged);

                var gameDto = openData as GameDto;
                _profileDto = _profileUseCase.GetProfile();
                _gameDto = gameDto;
                _windowHandler.OpenWindow(WindowType.CardsPanel, _gameDto);
                _windowHandler.OpenWindow(WindowType.TimerPanel);
                _windowHandler.OpenWindow(WindowType.GameBoostersPanel, _gameDto.Your.BoosterCards);

                _myAvatarSprite = _gameResourcesUseCase.GetCardSpriteByUrl(_profileDto.PersonalInfo.Avatar);
                _opponentAvatarSprite = _gameResourcesUseCase.GetCardSpriteByUrl(_gameDto.Other.AvatarImageUrl);

                ConcreteView.UpdateYourProfile(_profileDto.PersonalInfo.NickName, _myAvatarSprite);
                ConcreteView.UpdateOpponentProfile(_gameDto.Other.NickName, _opponentAvatarSprite);

                _gameEndUseCase.SubscribeOnPartyFinish(HandlePartyFinished);

                _gameplayUseCase.ReadyForParty();
                WindowHandler.CloseWindow(WindowType.LoadingScreen);
                _audioPlayer.PlayMusic(MusicType.BattleTheme);
                ConcreteView.SetRseTokensBalance(_profileDto.RseTokens);
            }

            public override void OnClose()
            {
                _gameStateMachine.UnsubscribeFromStateMachine(OnStateChanged);
                _gameEndUseCase.CloseConnection();
                _windowHandler.CloseWindow(WindowType.TimerPanel);
                _windowHandler.CloseWindow(WindowType.CardsPanel);
                _windowHandler.CloseWindow(WindowType.GameBoostersPanel);
            }

            private void HandlePartyFinished(PartyFinishedEvent @event)
            {
                _gameEndUseCase.UnsubscribeOnPartyFinish(HandlePartyFinished);
                Close();
            }


            private void OnStateChanged(GameState state, object data)
            {
                switch (state)
                {
                    case GameState.Idle:
                        break;
                    case GameState.WaitForOpponentTurn:
                        ConcreteView.ShowEndOfTurn();
                        break;
                    case GameState.BattleStarted:
                        _winLoses.Clear();
                        ConcreteView.Initialize();
                        break;
                    case GameState.BattleFinished:
                        OnBattleFinishedHandler((BattleFinishedEvent) data);

                        break;
                    case GameState.Attack:
                        ConcreteView.SetNormalColor();
                        ConcreteView.HideEndOfTurn();
                        ConcreteView.HideShowDownView();
                        break;
                    case GameState.Defence:
                        ConcreteView.SetNormalColor();
                        ConcreteView.HideEndOfTurn();
                        ConcreteView.HideShowDownView();
                        break;
                    case GameState.SelectStat:
                        ConcreteView.SetNormalColor();
                        ConcreteView.HideEndOfTurn();
                        break;
                    case GameState.EndOfTurn:
                        ConcreteView.SetFightColor();
                        var endOfTurnData = (RoundEndEvent) data;
                        var showDownData = CreateShowdownData(endOfTurnData);
                        _winLoses.Add(endOfTurnData.Result == RoundResultDto.Win);
                        ConcreteView.OpenShowDownView(showDownData,
                            ((RoundEndEvent) data).Result == RoundResultDto.Win);
                        _gameCommandsUseCase.ClearLastUsedBoosters();
                        break;
                }
            }


            private ShowdownData CreateShowdownData(RoundEndEvent @event)
            {
                var otherCardId = @event.OtherCardId;
                var otherCardDto = _gameDto.Other.CharacterCards.FirstOrDefault(x => x.Id == otherCardId);

                var myCard = _gameDto.Your.CharacterCards.FirstOrDefault(x => x.Id == @event.YourCardId);
                var myStatDto = myCard.Stats.FirstOrDefault(x => x.Type == @event.ChallengeStat);
                var myBoosterData = new StatBoosterViewData();
                var opponentBoosterData = new StatBoosterViewData(); // ToDo: Add correct data after server updates

                List<StatBoosterCardDto> usedBoosters = new List<StatBoosterCardDto>();

                foreach (var boosterId in _gameCommandsUseCase.LastUsedBoosterIds)
                {
                    usedBoosters.Add(
                        (StatBoosterCardDto) _gameDto.Your.BoosterCards.FirstOrDefault(x => x.Id == boosterId));
                }

                if (usedBoosters.Count > 0)
                {
                    var booster = usedBoosters[0];
                    Sprite myBoosterSprite = null;//_gameResourcesUseCase.GetCardSpriteByUrl(booster.ImageUrl);
                    var boosterEffect = booster.Effects[0];

                    myBoosterData.SpriteValue = myBoosterSprite;
                    myBoosterData.Count = 1;
                    myBoosterData.StatType = (CardStatsType) boosterEffect.StatType;
                    myBoosterData.Type = (BoosterType) booster.Type;
                    myBoosterData.Id = booster.Id;
                    myBoosterData.Value = boosterEffect.Value;
                }

                opponentBoosterData.Value = @event.OtherStatBoostValue;
                opponentBoosterData.SpriteValue = null;//_opponentAvatarSprite;// Todo change to booster sprite. get from otherCardDto

                CardStatData myStatData = myStatDto.CreateCardStatDataFromStatDto();
                CardStatData opponentStatData = new CardStatData((CardStatsType)@event.ChallengeStat, @event.OtherStatLevel);
                
                var showdownData =
                    new ShowdownData(_profileDto.PersonalInfo.NickName, _gameDto.Other.NickName,
                        _opponentAvatarSprite, _myAvatarSprite, myStatData
                        ,opponentStatData, myBoosterData, opponentBoosterData);

                return showdownData;
            }

            #region Callbacks from View

            public void OnEvent(ButtonType buttonType)
            {
                switch (buttonType)
                {
                    case ButtonType.Close:
                        Close();
                        _windowHandler.OpenWindow(WindowType.MainMenuCentral);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
                }
            }

            #endregion


            private async void OnBattleFinishedHandler(BattleFinishedEvent data)
            {
                var finishData = data;
                var wins = 0;
                var loses = 0;
                _winLoses.ForEach(x =>
                {
                    if (x)
                    {
                        wins++;
                    }
                    else
                    {
                        loses++;
                    }
                });

                var myAvatar = _gameResourcesUseCase.GetCardSpriteByUrl(_profileDto.PersonalInfo.Avatar);
                var otherAvatar = _gameResourcesUseCase.GetCardSpriteByUrl(_gameDto.Other.AvatarImageUrl);
                var battleResultData = new BattleResultsData
                {
                    IsYouWinner = finishData.ResultDto == BattleResultDto.Win,
                    LoseCount = loses,
                    WinCount = wins,
                    MyAvatar = myAvatar,
                    OpponentAvatar = otherAvatar
                };

                await UniTask.Delay(TimeSpan.FromSeconds(2));
                ConcreteView.HideShowDownView();
                _windowHandler.CloseWindow(WindowType.CardsPanel);
                _windowHandler.CloseWindow(WindowType.GameBoostersPanel);
                _windowHandler.OpenWindow(WindowType.BattleResults, battleResultData);

                Close();
            }
        }

        public GameplayWindowPresenter()
        {
        }

        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<ButtonClick>();
        }

        public enum ButtonType
        {
            Close
        }

        public class ButtonClick : EventHub<ButtonClick, ButtonType>
        {
        }
    }
}