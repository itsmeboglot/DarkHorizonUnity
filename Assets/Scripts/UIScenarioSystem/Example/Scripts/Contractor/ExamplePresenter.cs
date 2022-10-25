using System;
using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Interface;
using UiScenario;
using UnityEngine;

namespace Prefabs.UIScenario.Example
{
    public class ExamplePresenter : Presenter
    {
        private class Controller : Controller<ExampleView>, BtnClick.ISubscribed
        {
            public Controller(ExampleView view, IWindowHandler windowHandler) : base(view, windowHandler)
            {
            }

            private void SetColor()
            {
                Debug.Log("Set Color!");
            }

            public void OnEvent(Buttons value)
            {
                switch (value)
                {
                    case Buttons.Close:
                        Close();
                        break;
                    case Buttons.SetColor:
                        SetColor();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        //=======================================================
        
        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<BtnClick>();
        }
        
        //=======================================================
        
        /// <summary>
        ///  Called when player clicks on buttons.
        /// </summary>
        public class BtnClick : EventHub<BtnClick, Buttons>
        {
        }
        
        public enum Buttons
        {
            Close,
            SetColor
        }
    }
}
