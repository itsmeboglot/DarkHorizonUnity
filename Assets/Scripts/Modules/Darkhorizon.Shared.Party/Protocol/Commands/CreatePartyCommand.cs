using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Commands
{
    public class CreatePartyCommand<TStartupConfiguration> : IRequestCommand
    {
        public TStartupConfiguration StartupConfiguration;
    }
}