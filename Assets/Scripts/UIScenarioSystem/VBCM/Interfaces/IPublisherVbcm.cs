namespace Core.VBCM.Interfaces
{
    public interface IPublisherVbcm<TEventHub>
        where TEventHub : EventHubBase<TEventHub>
    {
        EventSource<TEventHub> GetEventSource1();
    }
    
    public interface IPublisherVbcm<TEventHub1, TEventHub2>
        where TEventHub1 : EventHubBase<TEventHub1>
        where TEventHub2 : EventHubBase<TEventHub2>
    {
        EventSource<TEventHub1> GetEventSource1();
        EventSource<TEventHub2> GetEventSource2();
    }
    
    public interface IPublisherVbcm<TEventHub1, TEventHub2, TEventHub3>
        where TEventHub1 : EventHubBase<TEventHub1>
        where TEventHub2 : EventHubBase<TEventHub2>
        where TEventHub3 : EventHubBase<TEventHub3>
    {
        EventSource<TEventHub1> GetEventSource1();
        EventSource<TEventHub2> GetEventSource2();
        EventSource<TEventHub3> GetEventSource3();
    }
    
    public interface IPublisherVbcm<TEventHub1, TEventHub2, TEventHub3, TEventHub4>
        where TEventHub1 : EventHubBase<TEventHub1>
        where TEventHub2 : EventHubBase<TEventHub2>
        where TEventHub3 : EventHubBase<TEventHub3>
        where TEventHub4 : EventHubBase<TEventHub4>
    {
        EventSource<TEventHub1> GetEventSource1();
        EventSource<TEventHub2> GetEventSource2();
        EventSource<TEventHub3> GetEventSource3();
        EventSource<TEventHub4> GetEventSource4();
    }
    
    public interface IPublisherVbcm<TEventHub1, TEventHub2, TEventHub3, TEventHub4, TEventHub5>
        where TEventHub1 : EventHubBase<TEventHub1>
        where TEventHub2 : EventHubBase<TEventHub2>
        where TEventHub3 : EventHubBase<TEventHub3>
        where TEventHub4 : EventHubBase<TEventHub4>
        where TEventHub5 : EventHubBase<TEventHub5>
    {
        EventSource<TEventHub1> GetEventSource1();
        EventSource<TEventHub2> GetEventSource2();
        EventSource<TEventHub3> GetEventSource3();
        EventSource<TEventHub4> GetEventSource4();
        EventSource<TEventHub5> GetEventSource5();
    }
}