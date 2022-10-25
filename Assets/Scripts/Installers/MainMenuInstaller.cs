using Unity.SceneLoaders;
using Zenject;

namespace Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainMenuLoader>().AsSingle().NonLazy();
        }
    }
}