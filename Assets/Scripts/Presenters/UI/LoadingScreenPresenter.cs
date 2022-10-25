using System;
using Core.UiScenario;
using Core.UiScenario.Interface;
using Views.UI;

namespace Presenters.UI
{
    public class LoadingScreenPresenter : Presenter
    {
        private class Controller : Controller<LoadingScreenView>
        {
            public Controller(LoadingScreenView view, IWindowHandler windowHandler) : base(view, windowHandler)
            {
            }
        }
        
        protected override void InstallBindings()
        {
            BindController<Controller>();
        }
    }
}
