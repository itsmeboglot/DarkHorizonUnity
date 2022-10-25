using Darkhorizon.Shared.Dto;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Commands.Battle
{
    public class AttackCommand : IRequestCommand
    {
        public int CardId;
        public StatTypeDto StatType;
        public int[] BoostersIds;
    }
}