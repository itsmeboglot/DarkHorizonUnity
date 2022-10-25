using Darkhorizon.Shared.Party.Dtos;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Lobby.Protocol.Events
{
    public class PartyInviteReceivedEvent : IResponseEvent
    {
        public string ServerUrl;
        public PartyTicketDto Ticket;
    }
}