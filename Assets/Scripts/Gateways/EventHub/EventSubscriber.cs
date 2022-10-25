using System;
using System.Reflection;
using Whimsy.Shared.Core;

namespace Gateways.EventHub
{
    /// <summary>
    ///     The container for subscribed delegate to the IServerEventHub.
    /// </summary>
    internal sealed class EventSubscriber
    {
        #region Constructors

        public EventSubscriber(Type type, Action<IResponseEvent> callback, MethodInfo methodInfo)
        {
            DeclaredType = type;
            Callback = callback;
            MethodInfo = methodInfo;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     The type that acts as a filter when announcing events.
        /// </summary>
        public Type DeclaredType { get; }

        /// <summary>
        ///     The delegate, that could be invoked by ServerEventHub.
        /// </summary>
        public Action<IResponseEvent> Callback { get; }

        /// <summary>
        ///     Additional subscriber delegate info, before ServerHub have casted to generic T.
        /// </summary>
        public MethodInfo MethodInfo { get; }

        #endregion
    }
}