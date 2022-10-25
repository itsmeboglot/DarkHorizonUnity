using System;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using UiScenario;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs.UIScenario.Example
{
    public class ExampleView : Presenter.View<ExamplePresenter>, IPublisher<ExamplePresenter.BtnClick>
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button setColorButton;
        
        public Func<ExamplePresenter.BtnClick> Event1 { private get; set; }
        
        public override void Initialize()
        {
            Binder.Bind(this);
            
            closeButton.onClick.AddListener(() => Event1().Publish(ExamplePresenter.Buttons.Close));
            setColorButton.onClick.AddListener(() => Event1().Publish(ExamplePresenter.Buttons.SetColor));
        }
    }
}
