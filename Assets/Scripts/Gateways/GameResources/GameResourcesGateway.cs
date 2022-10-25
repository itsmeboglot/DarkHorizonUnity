using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gateways.Interfaces;
using UnityEngine;
using UnityEngine.Networking;
using Utils;
using Utils.Logger;

namespace Gateways.Addressables
{
    public class GameResourcesGateway : IGameResourcesGateway
    {
        private readonly MainMono _mainMono;

        #region Fields

        private Dictionary<string, Sprite> _cardSpritesByUrlCache;
        private Dictionary<string, Sprite> _avatarSpritesByIdCache;
        // private SpritesDatabase _avatarsData;

        #endregion

        #region InstanceManagement

        public GameResourcesGateway(MainMono mainMono)
        {
            _mainMono = mainMono;
            _cardSpritesByUrlCache = new Dictionary<string, Sprite>();
            _avatarSpritesByIdCache = new Dictionary<string, Sprite>();
        }

        #endregion

        #region IGameResourcesGateway implementation

        public void ClearCardSpritesCache()
        {
            _cardSpritesByUrlCache.Clear();
            //Caching.ClearCache();
        }

        public Sprite GetSpriteByUrl(string url)
        {
            if (_cardSpritesByUrlCache.TryGetValue(url, out var sprite) || _avatarSpritesByIdCache.TryGetValue(url, out sprite))
            {
                return sprite;
            }
            
            CustomLogger.Log(LogSource.Unity, $"Cards cache don't contains url :{url}", MessageType.Warning);
            return null;
        }

        public void LoadAvatarSprites(IEnumerable<string> urls, Action doNext = null)
        {
            _mainMono.StartCoroutine(LoadImagesCor(urls, () => doNext?.Invoke()));
        }

        #endregion

        #region Private

        private async UniTask<Texture2D> GetTextureFromUrl(string url)
        {
            var www = UnityWebRequestTexture.GetTexture(url);
            var texture = await www.SendWebRequest().ToUniTask();
            var myTexture = DownloadHandlerTexture.GetContent(texture);
            return myTexture;
        }
        
        private IEnumerator LoadImagesCor(IEnumerable<string> urls, Action doNext)
        {
            foreach (var url in urls)
            {
                var hasInCardsCache = _avatarSpritesByIdCache.ContainsKey(url);
                var hasInAvatarsCache = _avatarSpritesByIdCache.ContainsKey(url);
                if (hasInCardsCache || hasInAvatarsCache)
                {
                    continue;
                }

                using var www = UnityWebRequestTexture.GetTexture(url);

                yield return www.SendWebRequest();
                Texture2D texture;
                try
                {
                    texture = DownloadHandlerTexture.GetContent(www);
                }
                catch (Exception e)
                {
                    texture = new Texture2D(2, 2);
                    CustomLogger.Log(LogSource.Server, $"Problem with URL {url}");
                    throw;
                }
                

                var sprite = Sprite.Create(texture,
                    new Rect(Vector2.zero, new Vector2(texture.width, texture.height)),
                    Vector2.zero);
                    
                _avatarSpritesByIdCache.Add(url, sprite);
            }
            
            doNext?.Invoke();
        }

        #endregion
    }
}