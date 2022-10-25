using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Commands.Battle
{
    public class DefendCommand : IRequestCommand
    {
        public int CardId;
        public int[] BoostersIds;
    }
}