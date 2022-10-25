using System.Collections.Generic;
using Darkhorizon.Shared.Party.Dtos;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class PartyCreatedEvent : IResponseEvent
    {
        #region Fields
        
        public IReadOnlyList<PartyTicketDto> Tickets;

        #endregion
    }
}