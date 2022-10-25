using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class BattleStartedEvent : IResponseEvent
    {
        public TurnType FirstToGo;
    }
}