using System.Collections.Generic;

namespace Whimsy.Shared.Core
{
    public class ServerResponse : IServerMessage
    {
        public ServerResponse()
        {
        }
        
        public IEnumerable<IResponseEvent> Events;
    }
}