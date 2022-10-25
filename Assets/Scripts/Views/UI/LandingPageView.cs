using System;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using Presenters.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class LandingPageView : Presenter.View<LandingPagePresenter>, IPublisher<LandingPagePresenter.ButtonClick>
    {
        [SerializeField] private Button loginButton;
        
        public Func<LandingPagePresenter.ButtonClick> Event1 { private get; set; }

        public override void Initialize()
        {
            Binder.Bind(this);
        }

        public override void OnPop()
        {
            loginButton.onClick.AddListener(() => Event1().Publish(LandingPagePresenter.ButtonType.Login));
        }

        public override void OnPush()
        {
            loginButton.onClick.RemoveAllListeners();
        }
    }
}