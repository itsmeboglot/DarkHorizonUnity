using Core.EventAggregator.Interface;
using Zenject;

namespace Core.EventAggregator
{
    /// <summary>
    /// Collecting knowledge from clever people ...
    /// </summary>
    public sealed class Subscriber : ISubscriber
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly DiContainer _container;

        public Subscriber(IEventAggregator eventAggregator, DiContainer container)
        {
            _eventAggregator = eventAggregator;
            _container = container;
        }
        
        public void Subscribe<TEvent>(EventHub<TEvent>.ISubscribed subscribed) where TEvent : EventHub<TEvent>
        {
            _eventAggregator.GetEvent<TEvent>(_container).Listen(subscribed);
        }
        
        public void UnSubscribe<TEvent>(EventHub<TEvent>.ISubscribed subscribed) where TEvent : EventHub<TEvent>
        {
            _eventAggregator.GetEvent<TEvent>(_container).UnListen(subscribed);
        }
        
        #region Subscribe

        public void Subscribe<TEvent, TValue>(EventHub<TEvent, TValue>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue>
        {
            _eventAggregator.GetEvent<TEvent>(_container).Listen(subscribed);
        }

        public void Subscribe<TEvent, TValue1, TValue2>(EventHub<TEvent,TValue1, TValue2>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue1, TValue2>
        {
            _eventAggregator.GetEvent<TEvent>(_container).Listen(subscribed);
        }

        public void Subscribe<TEvent, TValue1, TValue2, TValue3>(
            EventHub<TEvent,TValue1, TValue2, TValue3>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue1, TValue2, TValue3>
        {
            _eventAggregator.GetEvent<TEvent>(_container).Listen(subscribed);
        }

        #endregion Subscribe

        #region UnSubscribe

        public void UnSubscribe<TEvent, TValue>(EventHub<TEvent,TValue>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue>
        {
            _eventAggregator.GetEvent<TEvent>(_container).UnListen(subscribed);
        }

        public void UnSubscribe<TEvent, TValue1, TValue2>(EventHub<TEvent,TValue1, TValue2>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue1, TValue2>
        {
            _eventAggregator.GetEvent<TEvent>(_container).UnListen(subscribed);
        }

        public void UnSubscribe<TEvent, TValue1, TValue2, TValue3>(
            EventHub<TEvent,TValue1, TValue2, TValue3>.ISubscribed subscribed)
            where TEvent : EventHub<TEvent, TValue1, TValue2, TValue3>
        {
            _eventAggregator.GetEvent<TEvent>(_container).UnListen(subscribed);
        }

        #endregion UnSubscribe
    }
}