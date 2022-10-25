using Core.EventAggregator.Interface;
using Zenject;

namespace Core.EventAggregator
{
    public class EventAggregatorInstaller : Installer<EventAggregatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IEventAggregator>().To<Core.EventAggregator.EventAggregator>().AsSingle();
            Container.Bind<ISubscriber>().To<Subscriber>().AsSingle();
            Container.Bind<IBinder>().To<Binder>().AsSingle().NonLazy();
            Container.Bind<IValidator>().To<Validator>().AsSingle();
        }
    }
}