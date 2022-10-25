using System;
using System.Collections.Generic;

namespace Core.EventAggregator
{
    public abstract class EventHubBase
    {
        protected readonly List<WeakReference> Subscribers = new List<WeakReference>();

        public virtual void Listen<TSubscribed>(TSubscribed subscribed) where TSubscribed : class
        {
            var weakReference = new WeakReference(subscribed);
            Subscribers.Add(weakReference);
        }

        public virtual void UnListen<TSubscribed>(TSubscribed subscribed) where TSubscribed : class
        {
            Subscribers.RemoveAll(subWeak => !subWeak.IsAlive || subWeak.Target == subscribed);
        }
    }
}