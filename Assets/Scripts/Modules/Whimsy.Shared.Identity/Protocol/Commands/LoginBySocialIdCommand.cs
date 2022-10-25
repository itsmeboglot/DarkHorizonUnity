using Whimsy.Shared.Core;

namespace Whimsy.Shared.Identity.Protocol.Commands
{
    public class LoginBySocialIdCommand : IGuestRequestCommand
    {
        public string SocialNetName;
        public string SocialToken;
    }
}