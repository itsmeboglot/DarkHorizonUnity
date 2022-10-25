using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class ReadyForPartySucceedEvent : IResponseEvent
    {
        public static readonly ReadyForPartySucceedEvent Instance = new ReadyForPartySucceedEvent();
        
        private ReadyForPartySucceedEvent()
        {
            
        }
    }
}