using Darkhorizon.Shared.Dto;
using Darkhorizon.Shared.Player.Dto;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Player.Protocol.Events
{
    public class SacrificeSucceedEvent : IResponseEvent
    {
        public int SacrificedCardId;
        public CharacterCardDto NewCard;
    }
}