using System;
using System.Collections.Generic;
using System.Linq;
using Core.VBCM.Asserts;
using Core.VBCM.Interfaces;
using ModestTree;
using UnityEngine;

namespace Core.VBCM
{
    /// <summary>
    /// Base class for any Logic group management
    /// </summary>
    public class ControllerVbcm : IControllerVbcm
    {
        private readonly IDictionary<Type, List<WeakReference>> _handlerStorage =
            new Dictionary<Type, List<WeakReference>>();

        private readonly IDictionary<Type, List<WeakReference>> _callbackCommandStorage =
            new Dictionary<Type, List<WeakReference>>();

        /// <inheritdoc />
        public void Add<TEventHub>(EventHubBase<TEventHub>.IHandler handler)
            where TEventHub : EventHubBase<TEventHub>
        {
        }

        /// <inheritdoc />
        public void Remove<TCallBackValue, TEventHub, TSendValue>(
            EventHubVbcm<TCallBackValue, TEventHub, TSendValue>.IHandler handler)
            where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>
        {
        }

        /// <inheritdoc />
        public void AddCallBack<TCallBackValue, TEventHub, TSendValue>(Action<TCallBackValue> callBack)
            where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>
        {
        }

        /// <inheritdoc />
        public void RemoveCallBack<TCallBackValue, TEventHub, TSendValue>(Action<TCallBackValue> callBack)
            where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>
        {
        }

//======================================================
        /// <inheritdoc />
        public void Add<TEventHub, TSendValue>(EventHubVbcm<TEventHub, TSendValue>.IHandler handler)
            where TEventHub : EventHubVbcm<TEventHub, TSendValue>
        {
            var commandList = GetCommandList(typeof(TEventHub), _handlerStorage);
            commandList.Add(new WeakReference(handler));
        }

        /// <inheritdoc />
        public void Remove<TEventHub, TSendValue>(EventHubVbcm<TEventHub, TSendValue>.IHandler handler)
            where TEventHub : EventHubVbcm<TEventHub, TSendValue>
        {
            var commandList = GetCommandList(typeof(TEventHub), _handlerStorage);
            commandList.RemoveAll(com => com.Target == handler);
        }

        /// <inheritdoc />
        public void DoLogicWork<TEventHub, TSendValue>(TSendValue value, Type typeHub)
            where TEventHub : EventHubVbcm<TEventHub, TSendValue>
        {
            var commandList = GetCommandList(typeHub, _handlerStorage);
            if (!commandList.Any())
            {
                Debug.LogError("No action for this Hub: " + typeHub);
                return;
            }

            foreach (var weakReference in commandList)
            {
                var handler = (EventHubVbcm<TEventHub, TSendValue>.IHandler) weakReference.Target;
                handler.Handle(value);
            }
        }

//======================================================
        /// <inheritdoc />
        public void Add<TCallBackValue, TEventHub>(EventHubCallback<TCallBackValue, TEventHub>.IHandler handler)
            where TEventHub : EventHubCallback<TCallBackValue, TEventHub>
        {
            var commandList = GetCommandList(typeof(TEventHub), _handlerStorage);
            commandList.Add(new WeakReference(handler));
        }

        /// <inheritdoc />
        public void Remove<TCallBackValue, TEventHub>(EventHubCallback<TCallBackValue, TEventHub>.IHandler handler)
            where TEventHub : EventHubCallback<TCallBackValue, TEventHub>
        {
            var commandList = GetCommandList(typeof(TEventHub), _handlerStorage);
            commandList.RemoveAll(c => c.Target == handler);
        }

        /// <inheritdoc />
        public void DoLogicWork<TCallBackValue, TEventHub>(EventHubCallback<TCallBackValue, TEventHub>.IView view, Type typeHub)
            where TEventHub : EventHubCallback<TCallBackValue, TEventHub>
        {
            var commandList = GetCommandList(typeHub, _handlerStorage);
            if (!commandList.Any())
            {
                Debug.LogError("No action for this Hub: " + typeHub);
                return;
            }

            var lateCommandList = GetCommandList(typeHub, _callbackCommandStorage);

            foreach (var weakReference in commandList)
            {
                var handler = (EventHubCallback<TCallBackValue, TEventHub>.IHandler) weakReference.Target;
                var callBackValue = handler.Handle();
                view.SetCallbackValue(callBackValue);

                foreach (var lateWeakReference in lateCommandList)
                {
                    var action = (Action<TCallBackValue>) lateWeakReference.Target;
                    action.Invoke(callBackValue);
                }
            }
        }

        /// <inheritdoc />
        public void AddCallBack<TCallBackValue, TEventHub>(Action<TCallBackValue> callBack)
            where TEventHub : EventHubCallback<TCallBackValue, TEventHub>
        {
            var typeHub = typeof(TEventHub);
            var list = GetCommandList(typeHub, _callbackCommandStorage);
            list.Add(new WeakReference(callBack));
        }

        /// <inheritdoc />
        public void RemoveCallBack<TCallBackValue, TEventHub>(Action<TCallBackValue> callBack)
            where TEventHub : EventHubCallback<TCallBackValue, TEventHub>
        {
            var typeHub = typeof(TEventHub);
            var list = GetCommandList(typeHub, _callbackCommandStorage);
            var isRemove = list.Remove(new WeakReference(callBack));
            Assert.Warn(isRemove, AssertMessage.UnRegCallBack);
        }

//======================================================
        private static List<WeakReference> GetCommandList(Type subscriberType, IDictionary<Type, List<WeakReference>> target)
        {
            List<WeakReference> subscribers;
            var isExists = target.TryGetValue(subscriberType, out subscribers);
            if (isExists)
            {
                subscribers.RemoveAll(obj => !obj.IsAlive);
                return subscribers;
            }

            subscribers = new List<WeakReference>();
            target.Add(subscriberType, subscribers);

            return subscribers;
        }
    }
}