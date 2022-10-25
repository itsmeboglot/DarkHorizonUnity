using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class TimeReplenishedEvent : IResponseEvent
    {
        public int TimeLeft;
    }
}