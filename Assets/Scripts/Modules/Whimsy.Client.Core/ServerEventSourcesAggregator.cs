using System;
using System.Collections.Generic;
using System.Linq;
using Whimsy.Shared.Core;

namespace Whimsy.Client.Core
{
    public class ServerEventSourcesAggregator : IServerEventSource
    {
        #region Constructors

        public ServerEventSourcesAggregator(
            IEnumerable<IServerEventSource> sources)
        {
            _sources = sources
                .ToArray();

            foreach (var source in _sources)
                source.OnEvent += SourceOnEvent;
        }

        public static ServerEventSourcesAggregator Create(params IServerEventSource[] sources) =>
            new ServerEventSourcesAggregator(sources);

        #endregion

        #region Fields

        private readonly IServerEventSource[] _sources;

        #endregion

        #region Implementations

        public event Action<IResponseEvent> OnEvent;

        #endregion

        #region Utils

        private void SourceOnEvent(IResponseEvent @event) => OnEvent?.Invoke(@event);

        #endregion

        ~ServerEventSourcesAggregator()
        {
            foreach (var source in _sources)
                source.OnEvent -= SourceOnEvent;
        }
    }
}