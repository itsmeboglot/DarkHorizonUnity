using Darkhorizon.Shared.Dto;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class YourAutoAttackEvent : IResponseEvent
    {
        public int CardId;
        public StatTypeDto StatType;
    }
}