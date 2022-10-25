using System;
using System.Collections.Generic;
using Darkhorizon.Shared.Party.Protocol.Commands;
using Utils.Logger;
using Whimsy.Shared.Core;
using Zenject;

namespace Gateways
{
    using Connection;
    using EventHub;
    using Interfaces;

    public abstract class BaseSocketGateway : ISocketGateway, IInitializable, IDisposable
    {
        #region Fields

        private readonly SocketConnection _connection;
        protected readonly ServerEventHub eventHub = new ServerEventHub();

        #endregion

        #region InstanceManagement

        protected BaseSocketGateway(string serverUri)
        {
            _connection = new SocketConnection(serverUri);
        }

        #endregion

        #region Implementations

        
        public bool IsConnected => _connection.IsConnected;

        public IServerEventHub EventHub => eventHub;

        void IInitializable.Initialize()
        {
            Subscribe();
        }

        void IDisposable.Dispose()
        {
            UnSubscribe();
        }

        public void Connect()
        {
            _connection.Connect();
        }

        public void Disconnect()
        {
            _connection.Disconnect();
            ClearSubscribes();
        }

        public void Send(IRequestCommand command)
        {
            _connection.SendCommand(command);
        }
        
        public void ClearSubscribes()
        {
            eventHub.Clear();
        }

        #endregion

        #region InheritMethods

        protected virtual void HandleConnectionOpened()
        {
            Send(new EchoCommand(){Message = "Some echo message"});
        }

        protected virtual void HandleConnectionClosed()
        {
            CustomLogger.Log(LogSource.Unity, $"ConnectionClosed");
        }

        protected virtual void HandleConnectionError(string errorEventArgs)
        {
            CustomLogger.Log(LogSource.Server, errorEventArgs, MessageType.Error);
        }

        private void Subscribe()
        {
            _connection.OnOpen += HandleConnectionOpened;
            _connection.OnMessage += HandleConnectionMessage;
            _connection.OnError += HandleConnectionError;
            _connection.OnClose += HandleConnectionClosed;
        }

        private void UnSubscribe()
        {
            _connection.OnOpen -= HandleConnectionOpened;
            _connection.OnMessage -= HandleConnectionMessage;
            _connection.OnError -= HandleConnectionError;
            _connection.OnClose -= HandleConnectionClosed;
        }

        private void HandleConnectionMessage(IEnumerable<IResponseEvent> events)
        {
            eventHub.Notify(events);
        }

        #endregion
    }
}