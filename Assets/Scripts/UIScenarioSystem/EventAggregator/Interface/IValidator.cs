namespace Core.EventAggregator.Interface
{
    public interface IValidator
    {
        void Add<TEvent>(EventHubV<TEvent>.IValidated subscribed) where TEvent : EventHubV<TEvent>;

        void Remove<TEvent>(EventHubV<TEvent>.IValidated subscribed) where TEvent : EventHubV<TEvent>;

        void Add<TEvent, TValue>(EventHubV<TEvent, TValue>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue>;

        void Add<TEvent, TValue1, TValue2>(EventHubV<TEvent, TValue1, TValue2>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue1, TValue2>;

        void Add<TEvent, TValue1, TValue2, TValue3>(
            EventHubV<TEvent, TValue1, TValue2, TValue3>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue1, TValue2, TValue3>;

        void Remove<TEvent, TValue>(EventHubV<TEvent, TValue>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue>;

        void Remove<TEvent, TValue1, TValue2>(EventHubV<TEvent, TValue1, TValue2>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue1, TValue2>;

        void Remove<TEvent, TValue1, TValue2, TValue3>(
            EventHubV<TEvent, TValue1, TValue2, TValue3>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue1, TValue2, TValue3>;
    }
}