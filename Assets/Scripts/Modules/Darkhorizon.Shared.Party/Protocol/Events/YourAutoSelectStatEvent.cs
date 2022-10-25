using Darkhorizon.Shared.Dto;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class YourAutoSelectStatEvent : IResponseEvent
    {
        public StatTypeDto StatType;
    }
}