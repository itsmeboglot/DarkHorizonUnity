using Core.Unity;
using Unity.Settings;
using UnityEngine.SceneManagement;
using Zenject;

namespace Unity.SceneLoaders
{
    public class BootLoader : IInitializable
    {
        public void Initialize()
        {
            SceneManager.LoadScene(Const.Scenes.MainMenuScene);
        }
    }
}
