using Darkhorizon.Shared.Dto;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Commands.Battle
{
    public class SelectStatCommand : IRequestCommand
    {
        public StatTypeDto StatType;
        public int[] BoostersIds;
    }
}