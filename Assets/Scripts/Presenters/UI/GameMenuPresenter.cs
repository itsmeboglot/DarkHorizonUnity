using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using UnityEngine;
using Views.UI;

namespace Presenters.UI
{
    public class GameMenuPresenter : Presenter
    {
        private class Controller : Controller<GameMenuCentralView>, ButtonClick.ISubscribed
        {
            private readonly IWindowHandler _windowHandler;
            
            public Controller(GameMenuCentralView view, IWindowHandler windowHandler) : base(view, windowHandler)
            {
                _windowHandler = windowHandler;
            }

            public void OnEvent(ButtonType btnType)
            {
                switch (btnType)
                {
                    case ButtonType.UserProfile:
                        _windowHandler.OpenWindow(WindowType.ProfileCreation);
                        break;
                    case ButtonType.MenuPanel:
                        if (_windowHandler.IsOpen(WindowType.GameMenuWindow))
                        {
                            Close();
                            break;
                        }
                        _windowHandler.OpenWindow(WindowType.GameMenuWindow);
                        break;
                    case ButtonType.Shop:
                        Debug.Log("Pause btn clicked");
                        break;
                    case ButtonType.StartGame:
                        Debug.Log("Pause btn clicked");
                        break;
                    case ButtonType.StartTournament:
                        Debug.Log("Pause btn clicked");
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
            MenuPanel,
            UserProfile,
            StartGame,
            StartTournament,
            Shop,
            
        }
    }
}