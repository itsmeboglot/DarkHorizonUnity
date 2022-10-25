using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Whimsy.Shared.Core
{
    public class ServerRequest : IServerMessage
    {
        public static ServerRequest Create(IEnumerable<KeyValuePair<string, object>> header, IRequestCommand command)
        {
            if (command == null)
                throw new NullReferenceException(nameof(command));

            if (header == null)
                throw new NullReferenceException(nameof(header));

            return new ServerRequest
            {
                Command = command,
                Header = header
                    .ToDictionary(pair => pair.Key, pair => pair.Value)
            };
        }
        

        #region Fields

        public IDictionary<string, object> Header;
        public IRequestCommand Command;

        #endregion
    }
}