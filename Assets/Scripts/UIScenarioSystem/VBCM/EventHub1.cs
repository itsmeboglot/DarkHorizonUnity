using System;
using System.Collections.Generic;
using System.Linq;
using Core.VBCM.Helper;
using Core.VBCM.Interfaces;
using Zenject;

namespace Core.VBCM
{
    public abstract class EventHubVbcm<TEventHub, TSendValue> : EventHubBase<TEventHub>
        where TEventHub : EventHubVbcm<TEventHub, TSendValue>
    {
        private List<WeakReference> _handlers;
        private List<WeakReference> _validates;

        [Inject]
        private void Constructor(IView view, IValidator validator, IList<IHandler> handlers, IList<IValidate> validates)
        {
            _handlers = handlers.Select(handler => new WeakReference(handler)).ToList();
            _validates = validates.Select(validate => new WeakReference(validate)).ToList();
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
                var isValid = _validates.WeakCast<IValidate>().All(validated => validated.Validate(sendValue, out errorMessage));
                if (isValid)
                {
                    // Calling all handlers of the sent result
                    foreach (var handler in _handlers.WeakCast<IHandler>())
                        handler.Handle(sendValue);
                }
                else // If validation fails
                    view.NonValidAction(errorMessage);
            };
        }

        /// <summary>
        /// Add a function of the listener from the Ui event sources
        /// </summary>
        public void Controlled(IHandler handler)
        {
            _handlers.Add(new WeakReference(handler));
        }

        /// <summary>
        /// Remove a function of the listener from the Ui event sources
        /// </summary>
        public void UnControlled(IHandler handler)
        {
            _handlers.RemoveAll(obj => obj.IsAlive && obj.Target == handler);
        }

        /// <summary>
        /// Add unit for logic unit validation.
        /// </summary>
        public void Validate(IValidate validate)
        {
            _validates.Add(new WeakReference(validate));
        }

        /// <summary>
        /// Remove unit for logic unit validation.
        /// </summary>
        public void UnValidate(IValidate validate)
        {
            _validates.RemoveAll(obj => obj.IsAlive && obj.Target == validate);
        }

        public interface IValidate
        {
            bool Validate(TSendValue sendValue, out string errorMessage);
        }

        public interface IView
        {
            TSendValue SendValue { get; }
            void NonValidAction(string message);
        }

        public new interface IHandler
        {
            void Handle(TSendValue sendValue);
        }
    }
}