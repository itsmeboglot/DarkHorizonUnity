using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Core.EventAggregator
{
    public abstract class EventHub<TEvent, TValue1, TValue2> : EventHubBase
        where TEvent : EventHub<TEvent, TValue1, TValue2>
    {
        [Inject]
        private void Construct(IList<ISubscribed> subscribers)
        {
            foreach (var autoSubscriber in subscribers)
                Listen(autoSubscriber);
        }

        public virtual void Publish(TValue1 value1, TValue2 value2)
        {
            foreach (var subWeak in Subscribers.ToList())
            {
                if (subWeak.IsAlive)
                    ((ISubscribed) subWeak.Target).OnEvent(value1, value2);
                else
                    Subscribers.Remove(subWeak);
            }
        }

        public static TEvent operator +(EventHub<TEvent, TValue1, TValue2> eventHub, ISubscribed subscribed)
        {
            eventHub.Listen(subscribed);
            return (TEvent) eventHub;
        }

        public static TEvent operator -(EventHub<TEvent, TValue1, TValue2> eventHub, ISubscribed subscribed)
        {
            eventHub.UnListen(subscribed);
            return (TEvent) eventHub;
        }

        public interface ISubscribed
        {
            void OnEvent(TValue1 value1, TValue2 value2);
        }
    }

    public abstract class EventHubV<TEvent, TValue1, TValue2> : EventHub<TEvent, TValue1, TValue2>
        where TEvent : EventHubV<TEvent, TValue1, TValue2>
    {
        private readonly List<WeakReference> _validateds = new List<WeakReference>();

        [Inject]
        private void Construct(List<IValidated> publisheds)
        {
            foreach (var published in publisheds)
                AddValidator(published);
        }

        public override void Publish(TValue1 value1, TValue2 value2)
        {
            var list = _validateds.ToList();
            foreach (var subWeak in list)
            {
                if (subWeak.IsAlive)
                {
                    var validated = (IValidated) subWeak.Target;
                    var isValid = validated.Validate(value1, value2);
                    if (!isValid)
                        return;
                }
                else
                    _validateds.Remove(subWeak);
            }

            base.Publish(value1, value2);
        }

        public void AddValidator(IValidated validated)
        {
            _validateds.Add(new WeakReference(validated));
        }

        public void RemoveValidator(IValidated validated)
        {
            _validateds.RemoveAll(subWeak => !subWeak.IsAlive || subWeak.Target == validated);
        }

        public interface IValidated
        {
            bool Validate(TValue1 value1, TValue2 value2);
        }
    }
}