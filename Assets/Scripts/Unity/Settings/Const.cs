namespace Unity.Settings
{
    public static class Const
    {
        public static class Scenes
        {
            public const string BootScene = "Boot";
            public const string MainMenuScene = "MainMenu";
        }

        /// <summary>
        /// The const should be the same name as prefab
        /// </summary>
        public static class Poolables
        {
            public const string CollectionCard = "CollectionCard";
            public const string FighterCard = "FighterCard";
            public const string Avatar = "Avatar";
            public const string StatusNotification = "StatusNotification";
            public const string CardStat = "CardStat";
            public const string InGameBooster = "Booster";
        }

        public static class CardAnimatorValues
        {
            public const string UseCard = "UseCard";
            public const string ShowAvatar = "ShowAvatar";
            public const string FlipCard = "FlipCard";
            public const string HideGameBoard = "HideGameBoard";

        }
        
        public static class GameValues
        {
            public const int MaxCardsCount = 5;
            public const int MaxBoostersCount = 2;
        }
    }
}
