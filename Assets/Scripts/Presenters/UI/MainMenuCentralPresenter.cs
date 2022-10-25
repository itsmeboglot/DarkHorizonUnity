using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using Darkhorizon.Shared.Player.Dto;
using Game.Audio;
using UseCases.Addressables;
using UseCases.Menu;
using Views.UI;

namespace Presenters.UI
{
    public class MainMenuCentralPresenter : Presenter
    {
        private class Controller : Controller<MainMenuCentralView>, ButtonClick.ISubscribed
        {
            private readonly GameResourcesUseCase _gameResourcesUseCase;
            private readonly IAudioPlayer _audioPlayer;
            private readonly ProfileUseCase _profileUseCase;
            private ProfileDto _userProfile;

            public Controller(MainMenuCentralView view, IWindowHandler windowHandler,
                GameResourcesUseCase gameResourcesUseCase, IAudioPlayer audioPlayer,
                ProfileUseCase profileUseCase)
                : base(view, windowHandler)
            {
                _gameResourcesUseCase = gameResourcesUseCase;
                _audioPlayer = audioPlayer;
                _profileUseCase = profileUseCase;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                _userProfile = _profileUseCase.GetProfile();
                _profileUseCase.OnProfileUpdated += HandleProfileUpdate;
                UpdateView();
                _audioPlayer.PlayMusic(MusicType.MainTheme);
                ConcreteView.SetInteractable(true);    
            }

            public override void OnClose()
            {
                _profileUseCase.OnProfileUpdated -= HandleProfileUpdate;
            }

            void ButtonClick.ISubscribed.OnEvent(ButtonType buttonType)
            {
                switch (buttonType)
                {
                    case ButtonType.Close:
                        break;
                    case ButtonType.PreGame:
                        OpenPreGame();
                        break;
                    case ButtonType.Fighters:
                        OpenFighters();
                        break;
                }
            }

            private void HandleProfileUpdate(ProfileDto profile)
            {
                _userProfile = profile;
                UpdateView();
            }

            private void OpenPreGame()
            {
                // ConcreteView.SetInteractable(false);
                //Close();
                WindowHandler.OpenWindow(WindowType.PreGame);
            }
            
            private void OpenFighters()
            {
                // ConcreteView.SetInteractable(false);
                WindowHandler.OpenWindow(WindowType.FightersWindow, false);
            }

            private void UpdateView()
            {
                ConcreteView.UpdateProfileInfo(_userProfile.PersonalInfo.NickName,
                    _gameResourcesUseCase.GetCardSpriteByUrl(_userProfile.PersonalInfo.Avatar), _userProfile.RseTokens);
            }
        }

        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<ButtonClick>();
        }

        public class ButtonClick : EventHub<ButtonClick, ButtonType>
        {
        }

        public enum ButtonType
        {
            Close,
            PreGame,
            Fighters
        }
    }
}