using Darkhorizon.Shared.Dto;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class YourDefendEvent : IResponseEvent
    {
        public int AttackerCardId;
        public StatTypeDto AttackerStatType;
        public int TimeLeft;
        public bool BoosterWasUsed;
    }
}