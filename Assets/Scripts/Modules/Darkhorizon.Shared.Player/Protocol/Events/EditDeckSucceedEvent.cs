using Darkhorizon.Shared.Player.Dto;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Player.Protocol.Events
{
    public class EditDeckSucceedEvent : IResponseEvent
    {
        public DeckDto EditedDeckDto;
    }
}