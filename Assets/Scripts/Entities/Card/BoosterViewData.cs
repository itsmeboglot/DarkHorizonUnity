using UnityEngine;

namespace Entities.Card
{
    public class StatBoosterViewData : BoosterViewData
    {
        public CardStatsType StatType;
        public int Value;
        public int Count;
    }
    
    public abstract class BoosterViewData
    {
        public int Id;
        public Sprite SpriteValue;
        public BoosterType Type;
    }
    
    public enum BoosterType
    {
        SingleUse,
        MultipleUse
    }
}