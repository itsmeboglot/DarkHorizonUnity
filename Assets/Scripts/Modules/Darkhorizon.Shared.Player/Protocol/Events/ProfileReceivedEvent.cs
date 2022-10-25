using Darkhorizon.Shared.Player.Dto;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Player.Protocol.Events
{
    public class ProfileReceivedEvent : IResponseEvent
    {
        public ProfileDto Profile;
    }
}