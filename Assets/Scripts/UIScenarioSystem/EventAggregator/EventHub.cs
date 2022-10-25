using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Core.EventAggregator
{
    public abstract class EventHub<TEvent> : EventHubBase where TEvent : EventHub<TEvent>
    {
        [Inject]
        private void Construct(IList<ISubscribed> subscribers)
        {
            foreach (var subscriber in subscribers)
                Listen(subscriber);
        }

        public virtual void Publish()
        {
            foreach (var subWeak in Subscribers.ToList())
            {
                if (subWeak.IsAlive)
                    ((ISubscribed) subWeak.Target).OnEvent();
                else
                    Subscribers.Remove(subWeak);
            }
        }

        public static TEvent operator +(EventHub<TEvent> eventHub, ISubscribed subscribed)
        {
            eventHub.Listen(subscribed);
            return (TEvent) eventHub;
        }

        public static TEvent operator -(EventHub<TEvent> eventHub, ISubscribed subscribed)
        {
            eventHub.UnListen(subscribed);
            return (TEvent) eventHub;
        }

        public interface ISubscribed
        {
            void OnEvent();
        }
    }

    public abstract class EventHubV<TEvent> : EventHub<TEvent> where TEvent : EventHubV<TEvent>
    {
        private readonly List<WeakReference> _validateds = new List<WeakReference>();

        [Inject]
        private void Construct(List<IValidated> publisheds)
        {
            foreach (var published in publisheds)
                AddValidator(published);
        }

        public override void Publish()
        {
            var list = _validateds.ToList();
            foreach (var subWeak in list)
            {
                if (subWeak.IsAlive)
                {
                    var validated = (IValidated) subWeak.Target;
                    var isValid = validated.Validate();
                    if (!isValid)
                        return;
                }
                else
                    _validateds.Remove(subWeak);
            }

            base.Publish();
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
            bool Validate();
        }
    }

    public abstract class EventHubId<TEvent, TId> : EventHubBase where TEvent : EventHubId<TEvent, TId>
    {
        private readonly Dictionary<TId, WeakReference<ISubscribed>> _subscribers =
            new Dictionary<TId, WeakReference<ISubscribed>>();
        
        private readonly List<WeakReference> _validateds = new List<WeakReference>();

        [Inject]
        private void Construct(List<IValidated> validates)
        {
            foreach (var validated in validates)
                AddValidator(validated);
        }
        
        public void AddValidator(IValidated validated)
        {
            _validateds.Add(new WeakReference(validated));
        }
        
        public void Publish(TId id)
        {
            
            var list = _validateds.ToList();
            foreach (var subWeak in list)
            {
                if (subWeak.IsAlive)
                {
                    var validated = (IValidated) subWeak.Target;
                    var isValid = validated.Validate(id);
                    if (!isValid)
                        return;
                }
                else
                    _validateds.Remove(subWeak);
            }
            if (_subscribers.ContainsKey(id))
            {
                ISubscribed subscribed;
                if (_subscribers[id].TryGetTarget(out subscribed))
                    subscribed.OnEvent();
                else
                    _subscribers.Remove(id);
            }
        }

        public override void Listen<TSubscribed>(TSubscribed subscribed)
        {
            var subscribedId = (ISubscribed) subscribed;
            var weakReference = new WeakReference<ISubscribed>(subscribedId);
            _subscribers[subscribedId.Id] = weakReference;
        }

        public override void UnListen<TSubscribed>(TSubscribed subscribed)
        {
            var subscribedId = (ISubscribed) subscribed;
            _subscribers.Remove(subscribedId.Id);
        }

        public static TEvent operator +(EventHubId<TEvent, TId> eventHub, ISubscribed subscribed)
        {
            eventHub.Listen(subscribed);
            return (TEvent) eventHub;
        }

        public static TEvent operator -(EventHubId<TEvent, TId> eventHub, ISubscribed subscribed)
        {
            eventHub.UnListen(subscribed);
            return (TEvent) eventHub;
        }
        
        public interface ISubscribed
        {
            TId Id { get; }
            void OnEvent();
        }
        
        public interface IValidated
        {
            bool Validate(TId id);
        }
    }
}