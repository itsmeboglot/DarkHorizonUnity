namespace Core.EventAggregator.Interface
{
    public interface ISubscriber
    {
//        void Subscribe(object subscriber);
        void Subscribe<TEvent>(EventHub<TEvent>.ISubscribed subscribed) where TEvent : EventHub<TEvent>;

        void Subscribe<TEvent, TValue>(EventHub<TEvent, TValue>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue>;

        void Subscribe<TEvent, TValue1, TValue2>(EventHub<TEvent, TValue1, TValue2>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue1, TValue2>;

        void Subscribe<TEvent, TValue1, TValue2, TValue3>(
            EventHub<TEvent, TValue1, TValue2, TValue3>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue1, TValue2, TValue3>;

//        void UnSubscribe(object subscriber);
        void UnSubscribe<TEvent>(EventHub<TEvent>.ISubscribed subscribed) where TEvent : EventHub<TEvent>;

        void UnSubscribe<TEvent, TValue>(EventHub<TEvent, TValue>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue>;

        void UnSubscribe<TEvent, TValue1, TValue2>(EventHub<TEvent, TValue1, TValue2>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue1, TValue2>;

        void UnSubscribe<TEvent, TValue1, TValue2, TValue3>(
            EventHub<TEvent, TValue1, TValue2, TValue3>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue1, TValue2, TValue3>;
    }
}