using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class OpponentHasNotJoinedEvent : IResponseEvent
    {
        public static readonly OpponentHasNotJoinedEvent Instance = new OpponentHasNotJoinedEvent();
        
        private OpponentHasNotJoinedEvent()
        {
            
        }
    }
}