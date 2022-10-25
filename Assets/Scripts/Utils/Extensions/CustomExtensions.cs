using System.Collections.Generic;
using System.Linq;
using Darkhorizon.Shared.Party.Dtos;
using Darkhorizon.Shared.Player.Dto;
using Entities.Card;
using UnityEngine;
using SharedDto = Darkhorizon.Shared.Dto;
using PartyDto = Darkhorizon.Shared.Party.Dtos;

namespace Utils.Extensions
{
    public static class CustomExtensions
    {
        public static int GetExclusiveId(this SharedDto.CharacterCardDto[] cardDtos, List<int> excludeIds)
        {
            foreach (var cardDto in cardDtos)
            {
                if (!excludeIds.Contains(cardDto.Id))
                    return cardDto.Id;
            }

            Debug.LogError($"Can't get exclusive Id from deck!");
            return -1;
        }

        public static bool IsProfilesEquals(this ProfileDto self, ProfileDto otherProfile)
        {
            var isCardsCountEquals = self.CharacterCards.Length == otherProfile.CharacterCards.Length;
            var isBoostersCountEquals = self.BoosterCards.Length == otherProfile.BoosterCards.Length;

            if (isCardsCountEquals && isBoostersCountEquals)
                return false;
            
            //ToDo: implement equality
            
            return true;
        }
        
        public static Card CreateCardFromDto(this SharedDto.CharacterCardDto self)
        {
            var card = new Card();
            var stats = card.CardStats.FillSelfByStatDto(self.Stats);
            card.CardStats = stats;
            card.Id = self.Id;
            //ToDo: Set card name

            switch (self.Type)
            {
                case SharedDto.CharacterCardTypeDto.Bronze:
                    card.Power = 0;
                    break;
                case SharedDto.CharacterCardTypeDto.Silver:
                    card.Power = 1;
                    break;
                case SharedDto.CharacterCardTypeDto.Gold:
                    card.Power = 3;
                    break;
            }

            return card;
        }


        // public static Card CreateCardFromDto(this YourCharacterCardDto self)
        // {
        //     var card = new Card();
        //     var stats = card.CardStats.FillSelfByStatDto(self.Stats);
        //     card.CardStats = stats;
        //     card.Id = self.Id;
        //     //ToDo: Set card name
        //
        //     switch (self.Type)
        //     {
        //         case SharedDto.CharacterCardTypeDto.Bronze:
        //             card.Power = 0;
        //             break;
        //         case SharedDto.CharacterCardTypeDto.Silver:
        //             card.Power = 1;
        //             break;
        //         case SharedDto.CharacterCardTypeDto.Gold:
        //             card.Power = 3;
        //             break;
        //     }
        //
        //     return card;
        // }


        // private static CardStatData[] FillSelfByStatDto(this CardStatData[] self, SharedDto.StatDto[] dto)
        // {
        //     var stats = dto.Select(x => new CardStatData((CardStatsType) x.Type, x.Level));
        //     return stats.ToArray();
        // }
        //
        private static CardStatData[] FillSelfByStatDto(this CardStatData[] self, SharedDto.StatDto[] dto)
        {
            var stats = dto.Select(x => new CardStatData((CardStatsType) x.Type, x.Level));
            return stats.ToArray();
        }
        
        public static CardStatData FillSelfByStatDto(this CardStatData self, SharedDto.StatDto dto)
        {
            var stat = new CardStatData((CardStatsType) dto.Type, dto.Level);
            return stat;
        }
        
        public static CardStatData CreateCardStatDataFromStatDto(this SharedDto.StatDto dto)
        {
            var stat = new CardStatData((CardStatsType) dto.Type, dto.Level);
            return stat;
        }
    }
}