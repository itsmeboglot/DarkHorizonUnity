using Core.VBCM.Interfaces;
using Zenject;

namespace Core.VBCM
{
    public class VbcmInstaller : Installer<VbcmInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBinderVbcm>().To<BinderVbcm>().AsSingle();
            Container.Bind<IControllerVbcm>().To<ControllerVbcm>().AsSingle();
            Container.Bind<IValidator>().To<Validator>().AsSingle();
        }
    }
}