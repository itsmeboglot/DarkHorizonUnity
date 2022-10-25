using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Darkhorizon.Shared.Party.Protocol.Events;
using Utils.Logger;
using Whimsy.Shared.Core;
using Random = UnityEngine.Random;

namespace Gateways.EventHub
{
    public sealed class ServerEventHub : IServerEventHub
    {
        #region Fields

        private List<EventSubscriber> _subscribers = new List<EventSubscriber>();

        #endregion


        #region Implementations

        /// <summary>
        ///     IServerEventHub implementation. Subscribe your delegate to the server event.
        /// </summary>
        /// <param name="callback">The delegate, that will be invoked by ServerEventHub.</param>
        /// <typeparam name="T">Server event type, that implements IResponseEvent.</typeparam>
        public void Subscribe<T>(Action<T> callback) where T : IResponseEvent
        {
            var castedCallback = new Action<IResponseEvent>(x => callback((T) x));
            var observer = new EventSubscriber(typeof(T), castedCallback, callback.Method);
            _subscribers.Add(observer);
        }

        /// <summary>
        ///     IServerEventHub implementation. Unsubscribe your delegate from the server event.
        /// </summary>
        /// <param name="callback">The delegate you used to subscribe to IServerEventHub.</param>
        /// <typeparam name="T">Server event type, that implements IResponseEvent.</typeparam>
        public void Unsubscribe<T>(Action<T> callback) where T : IResponseEvent
        {
            for (var i = 0; i < _subscribers.Count; i++)
            {
                if (_subscribers[i].MethodInfo == callback.Method && _subscribers[i].DeclaredType == typeof(T))
                {
                    _subscribers.RemoveAt(i);
                    return;
                }
            }
        }
        
        public void Clear()
        {
            _subscribers.Clear();
        }

        #endregion

        #region InheritMethods

        /// <summary>
        ///     Notify method sends to subscribers subscribed event type data.
        /// </summary>
        /// <param name="events">List of events to notify about.</param>
        public async void Notify(IEnumerable<IResponseEvent> events)
        {
            foreach (var @event in events)
            {
                if (@event.GetType() == typeof(YourDefendEvent)
                    || @event.GetType() == typeof(OtherAttackEvent) || @event.GetType() == typeof(OtherDefendEvent))
                    await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(2f, 5f)));
                Notify(@event);
            }
        }

        public void Notify(IResponseEvent @event)
        {
            CustomLogger.Log( LogSource.Server, @event.GetType().ToString());
            for (var i = 0; i < _subscribers.Count; i++)
            {
                if (_subscribers[i].DeclaredType == @event.GetType())
                {
                    _subscribers[i].Callback(@event);
                }
            }
        }

        #endregion
    }
}