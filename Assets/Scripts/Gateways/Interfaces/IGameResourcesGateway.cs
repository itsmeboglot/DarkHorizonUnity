using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gateways.Interfaces
{

    public interface IGameResourcesGateway
    {
        void ClearCardSpritesCache();
        Sprite GetSpriteByUrl(string url);
        void LoadAvatarSprites(IEnumerable<string> urls, Action doNext = null);
    }

}