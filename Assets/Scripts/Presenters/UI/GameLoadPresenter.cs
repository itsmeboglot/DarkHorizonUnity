using System;
using System.Linq;
using Core.UiScenario;
using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using Darkhorizon.Shared.Party.Dtos;
using Darkhorizon.Shared.Player.Dto;
using UseCases.Addressables;
using UseCases.Menu;
using Views.UI;

namespace Presenters.UI
{
    public class GameLoadPresenter : Presenter
    {
        private class Controller : Controller<GameLoadView>
        {
            private readonly GameResourcesUseCase _gameResourcesUseCase;

            public Controller(GameLoadView view, IWindowHandler windowHandler,
                GameResourcesUseCase gameResourcesUseCase) : base(view, windowHandler)
            {
                _gameResourcesUseCase = gameResourcesUseCase;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                var gameDto = openData as GameDto;

                FadeOpen(() =>
                {
                    WindowHandler.CloseWindow(WindowType.MatchFound);
                    var otherAvatarUrl = gameDto.Other.AvatarImageUrl;
                    var urls = gameDto.Other.CharacterCards.Select(x => x.ImageUrl).ToList();
                    var myCards = gameDto.Your.CharacterCards.Select(x => x.ImageUrl).ToList();
                    var myBoosters = gameDto.Your.BoosterCards.Select(x => x.ImageUrl).ToList();
                    var opponentBoosters = gameDto.Other.BoosterCards.Select(x => x.ImageUrl).ToList();

                    urls.Add(otherAvatarUrl);
                    urls.AddRange(myCards);
                    urls.AddRange(myBoosters);
                    urls.AddRange(opponentBoosters);
                    
                    _gameResourcesUseCase.LoadCardSprites(urls, () => { FadeClose(gameDto); });
                });
            }

            private void FadeClose(GameDto gameDto)
            {
                WindowHandler.OpenWindow(WindowType.GameplayWindow, gameDto);
                ConcreteView.Canvas.sortingOrder += 100;
                ConcreteView.FadeOut(Close);
            }

            private void FadeOpen(Action doNext)
            {
                ConcreteView.FadeIn(() =>
                {
                    doNext?.Invoke();
                });
            }
        }

        protected override void InstallBindings()
        {
            BindController<Controller>();
        }
    }
}