using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class YourSelectStatEvent : IResponseEvent
    {
        public int TimeLeft;
        public bool BoosterWasUsed;
    }
}