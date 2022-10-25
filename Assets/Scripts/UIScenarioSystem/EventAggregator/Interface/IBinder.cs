namespace Core.EventAggregator.Interface
{
    public interface IBinder
    {
        void Bind<TEvent>(IPublisher<TEvent> publisher) where TEvent : EventHubBase;

        void Bind<TEvent1, TEvent2>(IPublisher<TEvent1, TEvent2> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase;

        void Bind<TEvent1, TEvent2, TEvent3>(IPublisher<TEvent1, TEvent2, TEvent3> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase;

        void Bind<TEvent1, TEvent2, TEvent3, TEvent4>(IPublisher<TEvent1, TEvent2, TEvent3, TEvent4> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase;

        void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase;

        void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
            where TEvent6 : EventHubBase;

        void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
            where TEvent6 : EventHubBase
            where TEvent7 : EventHubBase;

        void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
            where TEvent6 : EventHubBase
            where TEvent7 : EventHubBase
            where TEvent8 : EventHubBase;

        void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
            where TEvent6 : EventHubBase
            where TEvent7 : EventHubBase
            where TEvent8 : EventHubBase
            where TEvent9 : EventHubBase;
        
        void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9, TEvent10>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9, TEvent10> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
            where TEvent6 : EventHubBase
            where TEvent7 : EventHubBase
            where TEvent8 : EventHubBase
            where TEvent9 : EventHubBase
            where TEvent10 : EventHubBase;
    }
}