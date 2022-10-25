using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class CardSpriteData
    {
        public Sprite spriteValue;
        public string spriteId;
    }
    
    public class CardsDatabase 
    {
        private List<CardSpriteData> cardsData = new List<CardSpriteData>();
        
        public Sprite GetCardSpriteById(string spriteId)
        {
            var sprite = cardsData.FirstOrDefault(x => x.spriteId.Equals(spriteId));
            if (sprite == null)
            {
#if UNITY_EDITOR
                Debug.LogError("Can't find sprite by " + spriteId + " id");
#endif
            }
            return sprite.spriteValue;
        }

        public void Add(Texture2D texture, string id)
        {
            if (cardsData.Find((x) => x.spriteId.Equals(id)) != null)
            {
                Debug.LogWarning("Card with Id:"+ id+ " already added");
            }
            
            var spriteValue = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.zero);
            cardsData.Add(new CardSpriteData
            {
                spriteId = id,
                spriteValue = spriteValue
            });
        }
    }
}