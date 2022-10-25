using Darkhorizon.Shared.Player.Dto;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Player.Protocol.Commands
{
    public class RegisterCommand : IUserRequestCommand
    {
        public PersonalInfoDto PersonalInfoDto;
    }
}