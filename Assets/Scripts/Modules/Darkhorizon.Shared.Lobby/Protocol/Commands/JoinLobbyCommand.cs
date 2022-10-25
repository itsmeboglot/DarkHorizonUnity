using Whimsy.Shared.Core;
using Whimsy.Shared.Identity;

namespace Darkhorizon.Shared.Lobby.Protocol.Commands
{
    public class JoinLobbyCommand : IRequestCommand
    {
        public UserToken UserToken;
    }
}