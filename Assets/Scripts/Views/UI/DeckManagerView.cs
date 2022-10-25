using System;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using Presenters.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class DeckManagerView : Presenter.View<DeckManagerPresenter>, IPublisher<DeckManagerPresenter.DeckClick>
    {
        [SerializeField] private Button closeBtn;
        // [SerializeField] private Button closeBtn;
        
        public Func<DeckManagerPresenter.DeckClick> Event1 { get; set; }

        public override void OnPop()
        {
            base.OnPop();
        }
    }
}