using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using Zenject;

namespace Unity.SceneLoaders
{
    public class MainMenuLoader : IInitializable
    {
        private readonly IWindowHandler _windowHandler;

        public MainMenuLoader(IWindowHandler windowHandler)
        {
            _windowHandler = windowHandler;
        }
        
        public void Initialize()
        {
            _windowHandler.OpenWindow(WindowType.Background);
            _windowHandler.OpenWindow(WindowType.LoginPanel);
            _windowHandler.OpenWindow(WindowType.StatusNotification);
        }
    }
}
