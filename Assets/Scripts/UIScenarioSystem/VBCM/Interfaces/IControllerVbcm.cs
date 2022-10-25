using System;

namespace Core.VBCM.Interfaces
{
    /// <summary>
    /// Class-manager as main logic operations provider
    /// </summary>
    public interface IControllerVbcm
    {
        /// <summary>
        /// Add a function of the listener from the Ui event sources
        /// </summary>
        void Add<TEventHub>(EventHubBase<TEventHub>.IHandler handler)
            where TEventHub : EventHubBase<TEventHub>;

        /// <summary>
        /// Remove a function of the listener from the Ui event sources
        /// </summary>
        void Remove<TCallBackValue, TEventHub, TSendValue>(
            EventHubVbcm<TCallBackValue, TEventHub, TSendValue>.IHandler handler)
            where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>;

        /// <summary>
        /// Register action for late binding and calculations
        /// </summary>
        void AddCallBack<TCallBackValue, TEventHub, TSendValue>(Action<TCallBackValue> callBack)
            where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>;

        /// <summary>
        /// Unregister action for late binding and calculations
        /// </summary>
        void RemoveCallBack<TCallBackValue, TEventHub, TSendValue>(Action<TCallBackValue> callBack)
            where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>;


//======================================================
        /// <summary>
        /// Add a function of the listener from the Ui event sources
        /// </summary>
        void Add<TEventHub, TSendValue>(EventHubVbcm<TEventHub, TSendValue>.IHandler handler)
            where TEventHub : EventHubVbcm<TEventHub, TSendValue>;

        /// <summary>
        /// Remove a function of the listener from the Ui event sources
        /// </summary>
        void Remove<TEventHub, TSendValue>(EventHubVbcm<TEventHub, TSendValue>.IHandler handler)
            where TEventHub : EventHubVbcm<TEventHub, TSendValue>;

        /// <summary>
        /// Main Logic work function
        /// </summary>
        void DoLogicWork<TEventHub, TSendValue>(TSendValue value, Type hubType)
            where TEventHub : EventHubVbcm<TEventHub, TSendValue>;

//======================================================
        /// <summary>
        /// Add a function of the listener from the Ui event sources
        /// </summary>
        void Add<TCallBackValue, TEventHub>(EventHubCallback<TCallBackValue, TEventHub>.IHandler handler)
            where TEventHub : EventHubCallback<TCallBackValue, TEventHub>;

        /// <summary>
        /// Remove a function of the listener from the Ui event sources
        /// </summary>
        void Remove<TCallBackValue, TEventHub>(EventHubCallback<TCallBackValue, TEventHub>.IHandler handler)
            where TEventHub : EventHubCallback<TCallBackValue, TEventHub>;

        /// <summary>
        /// Main Logic work function
        /// </summary>
        void DoLogicWork<TCallBackValue, TEventHub>(EventHubCallback<TCallBackValue, TEventHub>.IView view, Type hubType)
            where TEventHub : EventHubCallback<TCallBackValue, TEventHub>;

        /// <summary>
        /// Register action for late binding and calculations
        /// </summary>
        void AddCallBack<TCallBackValue, TEventHub>(Action<TCallBackValue> callBack)
            where TEventHub : EventHubCallback<TCallBackValue, TEventHub>;

        /// <summary>
        /// Unregister action for late binding and calculations
        /// </summary>
        void RemoveCallBack<TCallBackValue, TEventHub>(Action<TCallBackValue> callBack)
            where TEventHub : EventHubCallback<TCallBackValue, TEventHub>;
    }
}