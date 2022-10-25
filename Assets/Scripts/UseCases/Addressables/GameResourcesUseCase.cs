using System;
using System.Collections.Generic;
using Gateways.Interfaces;
using UnityEngine;

namespace UseCases.Addressables
{
    public class GameResourcesUseCase
    {
        private readonly IGameResourcesGateway _gameResourcesGateway;

        public GameResourcesUseCase (IGameResourcesGateway gameResourcesGateway)
        {
            _gameResourcesGateway = gameResourcesGateway;
        }

        #region Cards loading

        public void LoadCardSprites (IEnumerable<string> data, Action doNext = null)
        {
            _gameResourcesGateway.LoadAvatarSprites(data, doNext);
        }
        
        public Sprite GetCardSpriteByUrl (string url)
        {
            return _gameResourcesGateway.GetSpriteByUrl(url);
        }

        public void ClearCardSpritesCache()
        {
            _gameResourcesGateway.ClearCardSpritesCache();
        }

        #endregion
        
    }
}