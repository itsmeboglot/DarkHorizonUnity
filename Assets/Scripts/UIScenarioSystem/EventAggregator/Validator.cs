using Core.EventAggregator.Interface;
using Zenject;

namespace Core.EventAggregator
{
    public class Validator : IValidator
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly DiContainer _diContainer;

        public Validator(IEventAggregator eventAggregator, DiContainer diContainer)
        {
            _eventAggregator = eventAggregator;
            _diContainer = diContainer;
        }

        public void Add<TEvent>(EventHubV<TEvent>.IValidated subscribed) where TEvent : EventHubV<TEvent>
        {
            _eventAggregator.GetEvent<TEvent>(_diContainer).AddValidator(subscribed);
        }

        public void Remove<TEvent>(EventHubV<TEvent>.IValidated subscribed) where TEvent : EventHubV<TEvent>
        {
            _eventAggregator.GetEvent<TEvent>(_diContainer).RemoveValidator(subscribed);
        }

        #region Add

        public void Add<TEvent, TValue>(EventHubV<TEvent, TValue>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue>
        {
            _eventAggregator.GetEvent<TEvent>(_diContainer).AddValidator(subscribed);
        }

        public void Add<TEvent, TValue1, TValue2>(EventHubV<TEvent, TValue1, TValue2>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue1, TValue2>
        {
            _eventAggregator.GetEvent<TEvent>(_diContainer).AddValidator(subscribed);
        }

        public void Add<TEvent, TValue1, TValue2, TValue3>(
            EventHubV<TEvent, TValue1, TValue2, TValue3>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue1, TValue2, TValue3>
        {
            _eventAggregator.GetEvent<TEvent>(_diContainer).AddValidator(subscribed);
        }

        #endregion Add

        #region Remove

        public void Remove<TEvent, TValue>(EventHubV<TEvent, TValue>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue>
        {
            _eventAggregator.GetEvent<TEvent>(_diContainer).RemoveValidator(subscribed);
        }

        public void Remove<TEvent, TValue1, TValue2>(EventHubV<TEvent, TValue1, TValue2>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue1, TValue2>
        {
            _eventAggregator.GetEvent<TEvent>(_diContainer).RemoveValidator(subscribed);
        }

        public void Remove<TEvent, TValue1, TValue2, TValue3>(
            EventHubV<TEvent, TValue1, TValue2, TValue3>.IValidated subscribed)
            where TEvent : EventHubV<TEvent, TValue1, TValue2, TValue3>
        {
            _eventAggregator.GetEvent<TEvent>(_diContainer).RemoveValidator(subscribed);
        }

        #endregion Remove
    }
}