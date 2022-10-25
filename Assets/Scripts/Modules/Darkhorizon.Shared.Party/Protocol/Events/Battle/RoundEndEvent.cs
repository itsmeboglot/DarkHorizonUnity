using Darkhorizon.Shared.Dto;
using Darkhorizon.Shared.Party.Dtos;
using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class RoundEndEvent : IResponseEvent
    {
        public int YourCardId;
        public int OtherCardId;
        public int OtherStatLevel;
        public int OtherStatBoostValue;
        public StatTypeDto ChallengeStat;
        public RoundResultDto Result;
    }
}