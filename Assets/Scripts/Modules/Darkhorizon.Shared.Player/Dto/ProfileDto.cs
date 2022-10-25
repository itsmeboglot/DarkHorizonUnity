using System;
using Darkhorizon.Shared.Dto;

namespace Darkhorizon.Shared.Player.Dto
{
    public class ProfileDto
    {
        public PersonalInfoDto PersonalInfo;
        public CharacterCardDto[] CharacterCards;
        public BoosterCardDto[] BoosterCards = Array.Empty<BoosterCardDto>();
        public DeckDto[] Decks = Array.Empty<DeckDto>();
        public int NextSacrificeTime;
        public int RseTokens;
    }
}