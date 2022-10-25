using System;
using Whimsy.Shared.Core;

namespace Whimsy.Client.Core
{
    public interface IServerEventSource
    {
        event Action<IResponseEvent> OnEvent;
    }
}