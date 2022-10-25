using Core.EventAggregator.Interface;
using Zenject;

namespace Core.EventAggregator
{
    public class EventAggregator : IEventAggregator
    {
        public TEvent GetEvent<TEvent>(DiContainer container) where TEvent : EventHubBase
        {
            return container.Resolve<TEvent>();
        }
    }
}