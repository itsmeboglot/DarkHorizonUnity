using Darkhorizon.Shared.Player.Dto;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Player.Protocol.Commands
{
    public class EditDeckCommand : IRequestCommand
    {
        public DeckDto Deck;
    }
}