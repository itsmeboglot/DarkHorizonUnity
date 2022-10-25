using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using Unity;
using Unity.Settings;
using UseCases.Game;
using Views.UI;

namespace Presenters.UI
{
    public class LandingPagePresenter : Presenter
    {
        private class Controller : Controller<LandingPageView>, ButtonClick.ISubscribed
        {
            public Controller(LandingPageView view, IWindowHandler windowHandler) 
                : base(view, windowHandler)
            {
            }

            void ButtonClick.ISubscribed.OnEvent(ButtonType buttonType)
            {
                switch (buttonType)
                {
                    case ButtonType.Login:
                        WindowHandler.OpenWindow(WindowType.LoginPanel);
                        break;
                }
            }
        }
        
        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<ButtonClick>();
        }

        public class ButtonClick : EventHub<ButtonClick, ButtonType>
        {
            
        }
        
        public enum ButtonType
        {
            Login
        }
    }
}