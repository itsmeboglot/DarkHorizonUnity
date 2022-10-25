using System;
using System.Collections.Generic;
using System.Linq;
using Core.VBCM.Helper;
using Core.VBCM.Interfaces;
using Zenject;

namespace Core.VBCM
{
    public abstract class EventHubVbcm<TCallBackValue, TEventHub, TSendValue> : EventHubBase<TEventHub>
        where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>
    {
        private List<WeakReference> _handlers;
        private List<WeakReference> _validates;
        private List<WeakReference> _callBacks;
        
        [Inject]
        private void Constructor(IView view, IValidator validator, IList<IHandler> handlers, IList<IValidate> validates,
            IList<ICallBack> callBacks)
        {
            _handlers = handlers.Select(handler => new WeakReference(handler)).ToList();
            _validates = validates.Select(validate => new WeakReference(validate)).ToList();
            _callBacks = callBacks.Select(callBack => new WeakReference(callBack)).ToList();
            Action = () =>
            {
                var errorMessage = string.Empty;
                // Get simple data from UI
                var sendValue = view.SendValue;
                //==============================
                var otherValidates = validator.Remove<TEventHub>();
                if (otherValidates != null)
                    _validates.AddRange(otherValidates);
                
                // Check the result of all validators
                var isValid = _validates.WeakCast<IValidate>()
                    .All(validated => validated.Validate(sendValue, out errorMessage));
                if (isValid)
                {
                    foreach (var handler in _handlers.WeakCast<IHandler>())
                    {
                        // Calling all handlers of the sent result
                        var callBackValue = handler.Handle(sendValue);
                        // Setting the result of each handler to subscribers
                        view.SetCallbackValue(callBackValue);
                        foreach (var callBack in _callBacks.WeakCast<ICallBack>())
                            callBack.CallBack(callBackValue);
                    }
                }
                else // If validation fails
                    view.NonValidAction(errorMessage);
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
            _callBacks.RemoveAll(obj => obj.IsAlive && callBack == obj.Target);
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
            _handlers.RemoveAll(obj => obj.IsAlive && handler == obj.Target);
        }

        /// <summary>
        /// Add unit for logic unit validation.
        /// </summary>
        public void AddValidate(IValidate validate)
        {
            _validates.Add(new WeakReference(validate));
        }

        /// <summary>
        /// Remove unit for logic unit validation.
        /// </summary>
        public void RemoveValidate(IValidate validate)
        {
            _validates.RemoveAll(obj => obj.IsAlive && validate == obj.Target);
        }

        public interface IValidate
        {
            bool Validate(TSendValue sendValue, out string errorMessage);
        }

        public interface IView
        {
            TSendValue SendValue { get; }
            void NonValidAction(string message);
            void SetCallbackValue(TCallBackValue value);
        }

        public interface ICallBack
        {
            void CallBack(TCallBackValue callBackValue);
        }

        public new interface IHandler
        {
            TCallBackValue Handle(TSendValue sendValue);
        }
    }
}