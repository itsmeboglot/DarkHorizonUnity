using Whimsy.Shared.Core;

namespace Whimsy.Shared.Identity.Protocol.Commands
{
    public class LoginCommand : IGuestRequestCommand
    {
        public string IdentityName;
        public string LoginToken;
    }
}