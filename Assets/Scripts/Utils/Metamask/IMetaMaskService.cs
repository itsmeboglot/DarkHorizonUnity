using System;

namespace Utils.Metamask
{
    public interface IMetamaskService
    {
        event Action<string, string, string> MetaMaskConnectionDataReceived;
        event Action<string> MetaMaskConnectError;
        void ConnectToMetamask();
    }
}