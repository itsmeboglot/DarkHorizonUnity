using System;
using Core.VBCM.Interfaces;

namespace Core.VBCM
{
    public abstract class EventHubBase<TEventHub>
        where TEventHub : EventHubBase<TEventHub>
    {
        public Action Action { get; protected set; }

        /// <summary>
        /// Usual UI event based control binding
        /// </summary>
        public void Bind(EventSource<TEventHub> eventSource)
        {
            eventSource.Event += Action;
        }

        /// <summary>
        /// Usual UI event based control unbinding
        /// </summary>
        public void UnBind(EventSource<TEventHub> eventSource)
        {
            eventSource.Event -= Action;
        }
        
        public interface IHandler
        {
        }
    }
}