using Whimsy.Shared.Core;

namespace Whimsy.Shared.Identity.Protocol.Commands
{
    public class AssignGameToSocialAccountCommand : IUserRequestCommand
    {
        public string SocialNetName;
        public string SocialToken;
    }
}