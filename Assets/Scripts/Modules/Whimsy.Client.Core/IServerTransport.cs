using System;

namespace Whimsy.Client.Core
{
    public interface IServerTransport
    {
        void Send(byte[] data, Action<SendingResult> onCompleteCallback = null);
        event Action<byte[]> OnReceived;
    }
}