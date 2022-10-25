using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using Zenject;

namespace Unity.SceneLoaders
{
    public class GameSceneLoader : IInitializable
    {
        private readonly IWindowHandler _windowHandler;

        public GameSceneLoader(IWindowHandler windowHandler)
        {
            _windowHandler = windowHandler;
        }
        
        public void Initialize()
        {
            _windowHandler.OpenWindow(WindowType.GameMenuWindow);
        }
    }
}
