using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class BattleFinishedEvent : IResponseEvent
    {
        public BattleResultDto ResultDto;
    }
}