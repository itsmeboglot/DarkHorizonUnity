using System;
using System.Collections.Generic;
using System.Linq;
using BestHTTP;
using UnityEngine;
using Utils;
using Whimsy.Client.Core;
using Whimsy.Shared.Core;

namespace Gateways.Connection
{
    public class HttpTransport : IServerTransport
    {
        private readonly string _httpUri;
        private readonly int _timeoutInSeconds;
        private readonly IServerMessageSerializer _serverMessageSerializer;
        private ServerSerializer _serializer;

        public HttpTransport(string httpUri, int timeoutInSeconds = 10,
            IServerMessageSerializer serverMessageSerializer = null)
        {
            _httpUri = httpUri;
            _timeoutInSeconds = timeoutInSeconds;
            _serverMessageSerializer = serverMessageSerializer;
            _serializer = new ServerSerializer();
        }

        #region Implementations

        private event Action<byte[]> OnReceived;

        event Action<byte[]> IServerTransport.OnReceived
        {
            add => OnReceived += value;
            remove => OnReceived -= value;
        }

        void IServerTransport.Send(byte[] data, Action<SendingResult> resultCallback)
        {
            var request = new HTTPRequest(new Uri(_httpUri), HTTPMethods.Post, HandleResponse)
                {RawData = data, Timeout = TimeSpan.FromSeconds(_timeoutInSeconds)};
            request.Send();

            void HandleResponse(HTTPRequest originalRequest, HTTPResponse response)
            {
                if (!response.IsSuccess)
                {
                    resultCallback?.Invoke(SendingResult.Failed);
                    return;
                }

                InvokeReceived(response.Data);
                resultCallback?.Invoke(SendingResult.Succeed);
            }
        }

        #endregion

        #region InheritMethods

        private void InvokeReceived(byte[] data)
        {
            OnReceived?.Invoke(data);
        }

        #endregion
    }
}