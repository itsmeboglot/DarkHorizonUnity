using System;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using Presenters.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class GameMenuCentralView : Presenter.View<GameMenuPresenter>, IPublisher<GameMenuPresenter.ButtonClick>
    {
        [SerializeField] private Button menuBtn;
        [SerializeField] private Button userProfileBtn;
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button startTournamentBtn;
        [SerializeField] private Button shopBtn;
        public Func<GameMenuPresenter.ButtonClick> Event1 { get; set; }
        
        public override void Initialize()
        {
            Binder.Bind(this);
            
            menuBtn.onClick.AddListener(           () => Event1().Publish(GameMenuPresenter.ButtonType.MenuPanel));
            userProfileBtn.onClick.AddListener(    () => Event1().Publish(GameMenuPresenter.ButtonType.UserProfile));
            startGameBtn.onClick.AddListener(      () => Event1().Publish(GameMenuPresenter.ButtonType.StartGame));
            startTournamentBtn.onClick.AddListener(() => Event1().Publish(GameMenuPresenter.ButtonType.StartTournament));
            shopBtn.onClick.AddListener(           () => Event1().Publish(GameMenuPresenter.ButtonType.Shop));

        }
    }
}
