using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Core.EventAggregator
{
    public abstract class EventHub<TEvent, TValue> : EventHubBase
        where TEvent : EventHub<TEvent, TValue>
    {
        [Inject]
        private void Construct(IList<ISubscribed> subscribers)
        {
            foreach (var autoSubscriber in subscribers)
                Listen(autoSubscriber);
        }

        public virtual void Publish(TValue value)
        {
            foreach (var subWeak in Subscribers.ToList())
            {
                if (subWeak.IsAlive)
                    ((ISubscribed) subWeak.Target).OnEvent(value);
                else
                    Subscribers.Remove(subWeak);
            }
        }

        public static TEvent operator +(EventHub<TEvent, TValue> eventHub, ISubscribed subscribed)
        { 
            eventHub.Listen(subscribed);
            return (TEvent) eventHub;
        }

        public static TEvent operator -(EventHub<TEvent, TValue> eventHub, ISubscribed subscribed)
        {
            eventHub.UnListen(subscribed);
            return (TEvent) eventHub;    
        }

        public interface ISubscribed
        {
            void OnEvent(TValue value);
        }
    }

    public abstract class EventHubV<TEvent, TValue> : EventHub<TEvent, TValue>
        where TEvent : EventHubV<TEvent, TValue>
    {
        private readonly List<WeakReference> _validateds = new List<WeakReference>();

        [Inject]
        private void Construct(List<IValidated> publisheds)
        {
            foreach (var published in publisheds)
                AddValidator(published);
        }

        public override void Publish(TValue value)
        {
            var list = _validateds.ToList();
            foreach (var subWeak in list)
            {
                if (subWeak.IsAlive)
                {
                    var validated = (IValidated) subWeak.Target;
                    var isValid = validated.Validate(value);
                    if (!isValid)
                        return;
                }
                else
                    _validateds.Remove(subWeak);
            }

            base.Publish(value);
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
            bool Validate(TValue value);
        }
    }
}