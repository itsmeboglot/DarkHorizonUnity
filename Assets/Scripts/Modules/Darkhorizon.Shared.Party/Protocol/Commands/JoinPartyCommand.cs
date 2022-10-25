using Darkhorizon.Shared.Party.Dtos;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Commands
{
    public class JoinPartyCommand : IRequestCommand
    {
        #region Fields

        public PartyTicketDto Ticket;

        #endregion
    }
}