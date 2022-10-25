using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Player.Protocol.Commands
{
    public class GetProfileCommand : IUserRequestCommand
    {
        public static readonly GetProfileCommand Instance = new GetProfileCommand();
        
        private GetProfileCommand()
        {
            
        }
    }
}