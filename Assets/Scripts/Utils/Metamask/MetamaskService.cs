 using System;
using System.Runtime.InteropServices;
 using Newtonsoft.Json;
using UnityEngine;
using Utils.Logger;

namespace Utils.Metamask
{
    public class MetamaskService : MonoBehaviour, IMetamaskService
    {
        [DllImport("__Internal")]
        private static extern void connectToMetamask(string phrase);
        
        public event Action<string, string, string> MetaMaskConnectionDataReceived;
        public event Action<string> MetaMaskConnectError;
        
        private string _message = string.Empty;
        
        private void Start()
        {
#if UNITY_EDITOR_WIN
            InitializeService();
#endif
        }

        public void ConnectToMetamask()
        {
            _message = Guid.NewGuid().ToString();
            
#if UNITY_EDITOR_WIN
            SignPhrase(_message);
#else
            connectToMetamask(_message);
#endif
        }

        public void ConnectToMetamaskHandler(string dataJson)
        {
            var data = JsonConvert.DeserializeObject<MetamaskConnectionData>(dataJson);
            MetaMaskConnectionDataReceived?.Invoke(_message, data.account, data.sign);
            _message = "";
        }

        public void MetamaskErrorHandler(string errorJson)
        {
            Debug.Log($"Browser error received {errorJson}");

            var errorData = JsonConvert.DeserializeObject<BrowserErrorData>(errorJson);

            if (errorData.type == "ConnectToMetamask")
            {
                MetaMaskConnectError?.Invoke(errorData.message);
            }
        }

#if UNITY_EDITOR_WIN
        private void InitializeService()
        {
            //ToDo: open http server on 8081 port for applicationDataPath (Assets) 

            var localhostHandler = new EditorLocalhostHandler();
            localhostHandler.Init(8081);

            var httpListener = new EditorHttpListener();
            httpListener.StartListen(8082);
            httpListener.RegisterTestListener("ConnectToMetamask", ConnectToMetamaskHandler);
            httpListener.RegisterTestListener("MetamaskErrorHandler", MetamaskErrorHandler);
        }

        private void SignPhrase(string phrase)
        {
            OpenBrowserPage(
                $"?type=ConnectToMetamask&responsePort={8082}&phrase={phrase}");
        }

        //ToDo signt transactions

        private void OpenBrowserPage(string urlParameters = "test=test")
        {
            Application.OpenURL(
                $"http://localhost:{8081}/Test/html/metamask.html{urlParameters}");
        }
#endif
        
        private struct MetamaskConnectionData
        {
            public string account;
            public string sign;
        }
    }
}