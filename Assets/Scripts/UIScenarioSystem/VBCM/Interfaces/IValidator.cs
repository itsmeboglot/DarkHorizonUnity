using System;
using System.Collections.Generic;

namespace Core.VBCM.Interfaces
{
    /// <summary>
    /// Main logic validation interface
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Add unit for logic unit validation.
        /// </summary>
        void Add<TCallBackValue, TEventHub, TSendValue>(
            EventHubVbcm<TCallBackValue, TEventHub, TSendValue>.IValidate validate)
            where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>;

        /// <summary>
        /// Remove unit for logic unit validation.
        /// </summary>
        void Remove<TCallBackValue, TEventHub, TSendValue>(
            EventHubVbcm<TCallBackValue, TEventHub, TSendValue>.IValidate validate)
            where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>;
//============================================
        /// <summary>
        /// Add unit for logic unit validation.
        /// </summary>
        void Add<TEventHub, TSendValue>(
            EventHubVbcm<TEventHub, TSendValue>.IValidate validate)
            where TEventHub : EventHubVbcm<TEventHub, TSendValue>;

        /// <summary>
        /// Remove unit for logic unit validation.
        /// </summary>
        void Remove<TEventHub, TSendValue>(
            EventHubVbcm<TEventHub, TSendValue>.IValidate validate)
            where TEventHub : EventHubVbcm<TEventHub, TSendValue>;

        List<WeakReference> Remove<TEventHub>()
            where TEventHub : EventHubBase<TEventHub>;
    }
}