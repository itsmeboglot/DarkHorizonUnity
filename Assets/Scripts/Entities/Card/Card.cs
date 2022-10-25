namespace Entities.Card
{
    public class Card
    {
        public int Id;
        public string Name;
        public int Power;
        public CardStatData[] CardStats;
    }

    public class CardStatData
    {
        public int Value;
        public CardStatsType StatType;

        public CardStatData (CardStatsType type, int value)
        {
            StatType = type;
            Value = value;
        }

        public CardStatData()
        {
        }
    }

    public enum CardStatsType
    {
        Accuracy,
        Agility,
        Cunning,
        Defence,
        Intelligence,
        Stamina,
        Strength
    }

    /// <summary>
    /// Used for UI stat description.
    /// Will be the same as CardStatsType
    /// </summary>
    public enum CardStatsTypeShort
    {
        ACC,
        AGL,
        CUN,
        DEF,
        INT,
        STA,
        STR
    }
    
}
