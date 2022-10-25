using System;
using System.Collections.Generic;
using System.Linq;
using Whimsy.Shared.Core;
using Whimsy.Shared.Core.Protocol.Events;

namespace Whimsy.Client.Core
{
    public class ServicePoint :
        ISending,
        IServerEventSource
    {
        #region Constructors

        public ServicePoint(
            IEnumerable<IProtocolFeature> features,
            IServerTransport transport,
            IServerMessageSerializer serverMessageSerializer)
        {
            _features = features
                .ToArray();
            
            _transport = transport;
            _transport.OnReceived += TransportOnReceived;
            
            _serverMessageSerializer = serverMessageSerializer;

            InitializeFeatures();

            void InitializeFeatures()
            {
                foreach (var feature in _features)
                    feature.Initialize(this);
            }
        }

        #endregion

        #region Fields

        private readonly IEnumerable<IProtocolFeature> _features;
        private readonly IServerTransport _transport;
        private readonly IServerMessageSerializer _serverMessageSerializer;

        #endregion

        #region Implementations

        public void Send(IRequestCommand command, Action<SendingResult> onCompletedCallback = null)
        {
            var requestBytes = PrepareRequestBytes();
            _transport.Send(requestBytes, onCompletedCallback);

            byte[] PrepareRequestBytes()
            {
                var header = new RequestHeader();

                // Enrich header
                foreach (var feature in _features)
                    feature.EnrichHeader(command, header);

                // Prepare request bytes
                var request = ServerRequest.Create(header, command); 

                return _serverMessageSerializer.Serialize(request);
            }
        }

        public event Action<IResponseEvent> OnEvent;

        #endregion

        #region Utils

        private void TransportOnReceived(byte[] bytes)
        {
            if (OnEvent == null)
                return;

            var response = _serverMessageSerializer.Deserialize<ServerResponse>(bytes);
            
            if (response?.Events == null)
                return;
            InvokeOnEvent(response.Events);
        }

        private void InvokeOnEvent(IEnumerable<IResponseEvent> events)
        {
            foreach (var @event in events)
                OnEvent?.Invoke(@event);
        }

        #endregion
    }
}