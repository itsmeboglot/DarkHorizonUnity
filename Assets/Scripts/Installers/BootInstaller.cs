using Unity.SceneLoaders;
using Zenject;

namespace Installers
{
    public class BootInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BootLoader>().AsSingle().NonLazy();
        }
    }
}
