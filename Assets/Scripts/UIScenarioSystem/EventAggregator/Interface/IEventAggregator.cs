using Zenject;

namespace Core.EventAggregator.Interface
{
    public interface IEventAggregator
    {
        TEvent GetEvent<TEvent>(DiContainer container) where TEvent : EventHubBase;
    }
}