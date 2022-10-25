using Core.UiScenario;
using Core.UiScenario.Interface;
using Darkhorizon.Shared.Party.Dtos;
using Game.Audio;
using UseCases.Addressables;
using UseCases.Menu;
using Views.UI;

namespace Presenters.UI
{
    public class MatchFoundPresenter : Presenter
    {
        private class Controller : Controller<MatchFoundView>
        {
            private readonly GameResourcesUseCase _gameResourcesUseCase;
            private readonly IAudioPlayer _audioPlayer;
            private readonly ProfileUseCase _profileUseCase;

            public Controller(MatchFoundView view, IWindowHandler windowHandler, GameResourcesUseCase gameResourcesUseCase,
                IAudioPlayer audioPlayer, ProfileUseCase profileUseCase) 
                : base(view, windowHandler)
            {
                _gameResourcesUseCase = gameResourcesUseCase;
                _audioPlayer = audioPlayer;
                _profileUseCase = profileUseCase;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                var gameDto = openData as GameDto;
                var profileDto = _profileUseCase.GetProfile();
                
                ConcreteView.SetYourVisualData(profileDto.PersonalInfo.NickName, _gameResourcesUseCase.GetCardSpriteByUrl(profileDto.PersonalInfo.Avatar));
                ConcreteView.SetOpponentVisualData(gameDto.Other.NickName, _gameResourcesUseCase.GetCardSpriteByUrl(gameDto.Other.AvatarImageUrl));
                
                _audioPlayer.StopMusic();
            }
        }

        protected override void InstallBindings()
        {
            BindController<Controller>();
        }
    }
}