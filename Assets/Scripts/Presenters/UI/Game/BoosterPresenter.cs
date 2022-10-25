using System.Collections.Generic;
using System.Linq;
using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Interface;
using Core.View.ViewPool;
using Darkhorizon.Shared.Dto;
using Entities.Card;
using Unity.Settings;
using UseCases.Addressables;
using UseCases.Game;
using Views.Cards;

namespace Presenters.UI.Game
{
    public class BoosterPresenter : Presenter
    {
        private class Controller : Controller<BoosterPanelView>, BoosterButtonClick.ISubscribed, ButtonClick.ISubscribed
        {
            private readonly GameCommandsUseCase _gameCommandsUseCase;
            private readonly GameResourcesUseCase _gameResourcesUseCase;
            private readonly ViewPool _viewPool;
            private List<BoosterView> _boosterViews = new List<BoosterView>(3);
            private List<BoosterCardDto> _boosters = new List<BoosterCardDto>(3);

            public Controller(BoosterPanelView view, IWindowHandler windowHandler,
                GameCommandsUseCase gameCommandsUseCase, GameResourcesUseCase gameResourcesUseCase, ViewPool viewPool) : base(view, windowHandler)
            {
                _gameCommandsUseCase = gameCommandsUseCase;
                _gameResourcesUseCase = gameResourcesUseCase;
                _viewPool = viewPool;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                _boosters = ((BoosterCardDto[])openData).ToList();
                ClearBoosterViews();
                CreateBoosters(_boosters);
                ConcreteView.InitWithViews(_boosterViews);
            }

            public override void OnClose()
            {
                ClearBoosterViews();
            }

            public void OnEvent(int boosterId)
            {
                _gameCommandsUseCase.SendBoosterUsed(boosterId);
            }
            
            private void CreateBoosters(List<BoosterCardDto> yourBoosterCards)
            {
                foreach (var dto in yourBoosterCards)
                {
                    var boosterView = _viewPool.Pop<BoosterView>(Const.Poolables.InGameBooster, ConcreteView.BoosterContainer);
                    var sprite = _gameResourcesUseCase.GetCardSpriteByUrl(dto.ImageUrl);
                    
                    //ToDo: Get all effects
                    var firstEffect = ((StatBoosterCardDto) dto).Effects.First();
                    var boosterData = new StatBoosterViewData
                    {
                        Id = dto.Id,
                        SpriteValue = sprite,
                        Type = (BoosterType)dto.Type,
                        StatType = (CardStatsType)firstEffect.StatType,
                        Value = firstEffect.Value,
                        Count = 1
                    };
                    boosterView.SetData(boosterData);
                    _boosterViews.Add(boosterView);
                }
            }

            private void ClearBoosterViews()
            {
                _boosterViews.ForEach(x => _viewPool.Push(x));
                _boosterViews.Clear();
            }

            public void OnEvent()
            {
                _gameCommandsUseCase.SendReplenishTime();
            }
        }

        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<BoosterButtonClick>()
                .BindEvent<ButtonClick>();
        }

        public class BoosterButtonClick : EventHub<BoosterButtonClick, int>
        {
        }

        public class ButtonClick : EventHub<ButtonClick>
        {
        }
    }
}

