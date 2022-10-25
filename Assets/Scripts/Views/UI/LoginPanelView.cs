using System;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using Presenters.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class LoginPanelView : Presenter.View<LoginPanelPresenter>, IPublisher<LoginPanelPresenter.ButtonClick>
    {
        [SerializeField] private Button metaMaskButton;
        
        public Func<LoginPanelPresenter.ButtonClick> Event1 { private get; set; }

        public override void Initialize()
        {
            Binder.Bind(this);
        }
        
        public override void OnPop()
        {
            metaMaskButton.onClick.AddListener(() => Event1().Publish(LoginPanelPresenter.ButtonType.MetaMask));
        }
    }
}