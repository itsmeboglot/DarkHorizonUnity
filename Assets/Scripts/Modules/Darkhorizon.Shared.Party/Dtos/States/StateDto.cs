using Darkhorizon.Shared.Dto;

namespace Darkhorizon.Shared.Party.Dtos
{
    public abstract class StateDto
    {
        
    }

    public class NotStartedStateDto : StateDto
    {
        
    }

    public class YourAttackStateDto : StateDto
    {
        public int TimeLeft;
    }

    public class YourDefendStateDto : StateDto
    {
        public int TimeLeft;
        public StatTypeDto OtherAttackerStatType;
    }

    public class YourSelectStatDto : StateDto
    {
        public int TimeLeft;
        public StatDto[] OtherPlayedStats;
        public StatDto[] YourPlayedStats;
        public StatTypeDto OtherAttackerStatType;
    }
}