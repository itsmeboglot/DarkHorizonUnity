using System;
using System.Collections.Generic;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using Presenters.UI.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Cards
{
    public class BoosterPanelView : Presenter.View<BoosterPresenter>, IPublisher<BoosterPresenter.BoosterButtonClick, BoosterPresenter.ButtonClick>
    {
        [SerializeField] private Transform boosterContainer;
        [SerializeField] private Button replenishTimeButton;

        public Transform BoosterContainer => boosterContainer; 
        public Func<BoosterPresenter.BoosterButtonClick> Event1 { get; set; }
        public Func<BoosterPresenter.ButtonClick> Event2 { get; set; }

        public override void Initialize()
        {
            Binder.Bind(this);
        }
        
        public override void OnPop()
        {
            replenishTimeButton.onClick.AddListener(OnReplenishTimeBtnClick);
        }

        public override void OnPush()
        {
            replenishTimeButton.onClick.RemoveListener(OnReplenishTimeBtnClick);
        }

        public void InitWithViews(List<BoosterView> boosterViews)
        {
            boosterViews.ForEach(x => x.OnBoosterClick += OnBoosterClickHandler);
        }

        private void OnBoosterClickHandler(int boosterId)
        {
            Event1().Publish(boosterId);
        }
        
        private void OnReplenishTimeBtnClick()
        {
            Event2().Publish();
        }
    }
}