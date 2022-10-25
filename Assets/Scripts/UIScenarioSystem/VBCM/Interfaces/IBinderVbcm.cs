namespace Core.VBCM.Interfaces
{
    /// <summary>
    /// Translate logic to IController part
    /// </summary>
    public interface IBinderVbcm
    {
        /// <summary>
        /// Lazy subscription to the event current EventHub
        /// </summary>
        void Bind<TEventHub>(EventSource<TEventHub> eventSource)
            where TEventHub : EventHubBase<TEventHub>;

        /// <summary>
        /// Lazy unsubscription to the event current EventHub
        /// </summary>
        void UnBind<TEventHub>(EventSource<TEventHub> eventSource)
            where TEventHub : EventHubBase<TEventHub>;

        /// <summary>
        /// Lazy subscription to the event current EventHub
        /// </summary>
        void Bind<TEventHub>(IPublisherVbcm<TEventHub> publisher)
            where TEventHub : EventHubBase<TEventHub>;

        /// <summary>
        /// Lazy unsubscription to the event current EventHub
        /// </summary>
        void UnBind<TEventHub>(IPublisherVbcm<TEventHub> publisher)
            where TEventHub : EventHubBase<TEventHub>;

        #region BigPublishers

        /// <summary>
        /// Lazy subscription to the event current EventHub
        /// </summary>
        void Bind<TEventHub1, TEventHub2>(IPublisherVbcm<TEventHub1, TEventHub2> publisher)
            where TEventHub1 : EventHubBase<TEventHub1>
            where TEventHub2 : EventHubBase<TEventHub2>;

        void Bind<TEventHub1, TEventHub2, TEventHub3>(IPublisherVbcm<TEventHub1, TEventHub2, TEventHub3> publisher)
            where TEventHub1 : EventHubBase<TEventHub1>
            where TEventHub2 : EventHubBase<TEventHub2>
            where TEventHub3 : EventHubBase<TEventHub3>;

        void Bind<TEventHub1, TEventHub2, TEventHub3, TEventHub4>(
            IPublisherVbcm<TEventHub1, TEventHub2, TEventHub3, TEventHub4> publisher)
            where TEventHub1 : EventHubBase<TEventHub1>
            where TEventHub2 : EventHubBase<TEventHub2>
            where TEventHub3 : EventHubBase<TEventHub3>
            where TEventHub4 : EventHubBase<TEventHub4>;

        void Bind<TEventHub1, TEventHub2, TEventHub3, TEventHub4, TEventHub5>(
            IPublisherVbcm<TEventHub1, TEventHub2, TEventHub3, TEventHub4, TEventHub5> publisher)
            where TEventHub1 : EventHubBase<TEventHub1>
            where TEventHub2 : EventHubBase<TEventHub2>
            where TEventHub3 : EventHubBase<TEventHub3>
            where TEventHub4 : EventHubBase<TEventHub4>
            where TEventHub5 : EventHubBase<TEventHub5>;

        #endregion
    }
}