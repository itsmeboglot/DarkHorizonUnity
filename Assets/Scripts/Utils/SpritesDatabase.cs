using System.Linq;
using Core.Helpers;
using UnityEngine;

namespace Utils
{
    public class SpritesDatabase : ScriptableObject
    {
        [SerializeField] private Sprite[] spritesData;

        public bool IsEmpty => spritesData.Length == 0;

        public Sprite[] GetAllSprites()
        {
            return spritesData;
        }

        public Sprite GetSpriteByIndex(int index)
        {
            return spritesData[index];
        }

        #region Editor Utilities

#if UNITY_EDITOR
        // [ContextMenu("AddAllSelectedSprites")]
        // private void AddAllSelectedSprites()
        // {
        //     spritesData = new Sprite[UnityEditor.Selection.objects.Length];
        //     int index = 0;
        //
        //     var sprites = UnityEditor.Selection.GetFiltered<Sprite>(UnityEditor.SelectionMode.Unfiltered);
        //     foreach (var sprite in sprites)
        //     {
        //         spritesData[index] = sprite;
        //         //ToDo: Add selected sprite to spritesData. Use for sprites with Id
        //
        //         index++;
        //     }
        // }
#endif

        #endregion
    }
}