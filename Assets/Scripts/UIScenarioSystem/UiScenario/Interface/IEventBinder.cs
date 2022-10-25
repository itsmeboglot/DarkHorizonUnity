using Core.EventAggregator;

namespace Core.UiScenario.Interface
{
    public interface IEventBinder
    {
        IEventBinder BindEvent<TEvent>() where TEvent : EventHubBase;
    }
}