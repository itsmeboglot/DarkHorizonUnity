using System;
using UnityEngine;
using Utils.Metamask;

namespace UseCases.Menu
{
    public class ObserveMetaMaskConnectUseCase
    {
        private readonly IMetamaskService _metamaskService;

        public ObserveMetaMaskConnectUseCase(IMetamaskService metamaskService)
        {
            _metamaskService = metamaskService;
        }

        public void Connect(Action<string, string, string> onComplete = null)
        {
            if (onComplete != null)
                _metamaskService.MetaMaskConnectionDataReceived += onComplete;

#if !UNITY_EDITOR_OSX
            _metamaskService.ConnectToMetamask();
#else
            onComplete?.Invoke("9e13f047-0b3b-4bf3-8b47-ac8d89a0c4da",
                "0x4bb8113eb564f09286e387f0546ab06992d2844c11395742fa989651c786eb50793a047b022d8289d90c2f004b515a087dd93dbd0ba201a47fdecdcf50a38b111c",
                "0x4bb8113eb564f09286e387f0546ab06992d2844c11395742fa989651c786eb50793a047b022d8289d90c2f004b515a087dd93dbd0ba201a47fdecdcf50a38b111c");
#endif
        }

        public void DisposeSubscribe(Action<string, string, string> onComplete)
        {
            if (onComplete != null)
                _metamaskService.MetaMaskConnectionDataReceived -= onComplete;
        }
    }
}