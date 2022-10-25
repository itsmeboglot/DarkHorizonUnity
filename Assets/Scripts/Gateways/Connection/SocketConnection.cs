using System;
using System.Collections.Generic;
using BestHTTP;
using BestHTTP.WebSocket;
using Cysharp.Threading.Tasks;
using Utils;
using Utils.Logger;
using Whimsy.Shared.Core;

namespace Gateways.Connection
{
    public class SocketConnection
    {
        #region Events

        public event Action OnOpen;
        public event Action OnClose;
        public event Action<string> OnError;
        public event Action<List<IResponseEvent>> OnMessage;

        #endregion

        #region Fields

        private WebSocket _webSocket;
        private readonly string _serverUri;
        private readonly ServerSerializer _serializer = new ServerSerializer();

        #endregion

        #region InstanceManagement

        public SocketConnection(string serverUri)
        {
            _serverUri = serverUri;
        }

        #endregion

        #region Properties

        public bool IsConnected { get; private set; }

        #endregion

        #region PublicMethods

        public void Connect()
        {
            _webSocket = new WebSocket(new Uri(_serverUri));
            Subscribe();
            _webSocket.Open();
        }

        public void SendCommand(IRequestCommand command)
        {
            var commandBytes = _serializer.Serialize(command);
            _webSocket.Send(commandBytes);
        }

        public void Disconnect()
        {
            // if (_webSocket.IsOpen)
            // {
                _webSocket.Close();
            // }
        }

        #endregion

        #region InheritMethods

        private void Subscribe()
        {
            if (_webSocket == null) return;

            _webSocket.OnOpen = HandleSocketOpen;
            _webSocket.OnBinary = HandleSocketMessage;
            _webSocket.OnError = HandleSocketError;
            _webSocket.OnClosed = HandleSocketClose;
        }

        private void UnSubscribe()
        {
            if (_webSocket == null) return;

            _webSocket.OnOpen = null;
            _webSocket.OnBinary = null;
            _webSocket.OnError = null;
            _webSocket.OnClosed = null;
        }

        private void HandleSocketOpen(WebSocket ws)
        {
            if (ws != _webSocket)
                return;
            
            IsConnected = true;
            OnOpen?.Invoke();
        }

        private void HandleSocketClose(WebSocket ws, ushort code, string message)
        {
            if (ws != _webSocket)
                return;
   
            IsConnected = false;
            UnSubscribe();
            OnClose?.Invoke();
        }

        private void HandleSocketMessage(WebSocket ws, byte[] message)
        {
            if (ws != _webSocket)
                return;
            
            var events = _serializer.Deserialize<List<IResponseEvent>>(message);
            if(events != null)
                OnMessage?.Invoke(events);
        }

        private void HandleSocketError(WebSocket ws, string reason)
        {
            if (ws != _webSocket)
                return;

#if !UNITY_WEBGL || UNITY_EDITOR
            if (string.IsNullOrEmpty(reason))
            {
                switch (ws.InternalRequest.State)
                {
                    // The request finished without any problem.
                    case HTTPRequestStates.Finished:
                        if (ws.InternalRequest.Response.IsSuccess || ws.InternalRequest.Response.StatusCode == 101)
                            reason = string.Format("Request finished. Status Code: {0} Message: {1}", ws.InternalRequest.Response.StatusCode.ToString(), ws.InternalRequest.Response.Message);
                        else
                            reason = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                            ws.InternalRequest.Response.StatusCode,
                                                            ws.InternalRequest.Response.Message,
                                                            ws.InternalRequest.Response.DataAsText);
                        break;

                    // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
                    case HTTPRequestStates.Error:
                        reason = "Request Finished with Error! : " + ws.InternalRequest.Exception != null ? (ws.InternalRequest.Exception.Message + " " + ws.InternalRequest.Exception.StackTrace) : string.Empty;
                        break;

                    // The request aborted, initiated by the user.
                    case HTTPRequestStates.Aborted:
                        reason = "Request Aborted!";
                        break;

                    // Connecting to the server is timed out.
                    case HTTPRequestStates.ConnectionTimedOut:
                        reason = "Connection Timed Out!";
                        break;

                    // The request didn't finished in the given time.
                    case HTTPRequestStates.TimedOut:
                        reason = "Processing the request Timed Out!";
                        break;
                }
            }
#endif
            CustomLogger.Log(LogSource.Unity,$"Connection ERROR: {reason}", MessageType.Error);
            OnError?.Invoke(reason);
        }

        #endregion
    }
}