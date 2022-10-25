using System;
using Whimsy.Shared.Core;

namespace Whimsy.Client.Core
{
    public interface ISending
    {
        void Send(IRequestCommand command, Action<SendingResult> onCompletedCallback = null);
    }
}