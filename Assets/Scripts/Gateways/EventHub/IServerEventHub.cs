using System;
using Whimsy.Shared.Core;

namespace Gateways.EventHub
{
    public interface IServerEventHub
    {
        void Subscribe<T>(Action<T> callback) where T : IResponseEvent;
        void Unsubscribe<T>(Action<T> callback) where T : IResponseEvent;
    }
}