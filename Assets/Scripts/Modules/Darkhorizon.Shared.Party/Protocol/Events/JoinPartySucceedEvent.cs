using Darkhorizon.Shared.Party.Dtos;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class JoinPartySucceedEvent : IResponseEvent
    {
        public GameDto Game;
    }
}