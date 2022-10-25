using System;
using System.Collections.Generic;
using System.Linq;
using Core.VBCM.Helper;
using Zenject;

namespace Core.VBCM
{
    public abstract class EventHubCallback<TCallBackValue, TEventHub> : EventHubBase<TEventHub>
        where TEventHub : EventHubCallback<TCallBackValue, TEventHub>
    {
        private IView _view;
        private List<WeakReference> _handlers;
        private List<WeakReference> _callBacks;

        [Inject]
        private void Constructor(IView view, IList<IHandler> handlers, IList<ICallBack> callBacks)
        {
            _handlers = handlers.Select(handler => new WeakReference(handler)).ToList();
            _callBacks = callBacks.Select(callBack => new WeakReference(callBack)).ToList();
            Action = () =>
            {
                foreach (var handler in _handlers.WeakCast<IHandler>())
                {
                    // Calling all handlers of the sent result
                    var callBackValue = handler.Handle();
                    // Setting the result of each handler to subscribers
                    view.SetCallbackValue(callBackValue);
                    foreach (var callBack in _callBacks.WeakCast<ICallBack>())
                        callBack.CallBack(callBackValue);
                }
            };
        }

        /// <summary>
        /// Used for delayed execution by external event
        /// </summary>
        public void AddCallBack(ICallBack callBack)
        {
            _callBacks.Add(new WeakReference(callBack));
        }

        /// <summary>
        /// Unused for delayed execution by external event
        /// </summary>
        public void RemoveCallBack(ICallBack callBack)
        {
            _callBacks.RemoveAll(obj => obj.IsAlive && obj.Target == callBack);
        }

        /// <summary>
        /// Add a function of the listener from the Ui event sources
        /// </summary>
        public void AddHandler(IHandler handler)
        {
            _handlers.Add(new WeakReference(handler));
        }

        /// <summary>
        /// Remove a function of the listener from the Ui event sources
        /// </summary>
        public void RemoveHandler(IHandler handler)
        {
            _handlers.RemoveAll(obj => obj.IsAlive && obj.Target == handler);
        }

        public interface IView
        {
            void SetCallbackValue(TCallBackValue value);
        }

        public interface ICallBack
        {
            void CallBack(TCallBackValue callBackValue);
        }

        public new interface IHandler
        {
            TCallBackValue Handle();
        }
    }
}