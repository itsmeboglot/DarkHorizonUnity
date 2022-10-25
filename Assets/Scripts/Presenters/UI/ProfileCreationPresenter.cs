using System.Linq;
using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using Cysharp.Threading.Tasks;
using Darkhorizon.Shared.Player.Dto;
using Darkhorizon.Shared.Player.Protocol.Events;
using Entities.User;
using UnityEngine;
using UseCases.Addressables;
using UseCases.Menu;
using Utils.Tweens;
using Views.UI;

namespace Presenters.UI
{
    public class ProfileCreationPresenter : Presenter
    {
        private class Controller : Controller<ProfileCreationView>,
            ButtonClick.ISubscribed, TextInput.ISubscribed, AvatarVariantButtonClick.ISubscribed
        {
            private string _enteredNickName;
            private string _enteredBio;
            private string _selectedAvatar;
            private AvatarData[] _avatarDatas;
            private WalletNftCollectionDto _nftCollection;

            private readonly IWindowHandler _windowHandler;
            private readonly GameResourcesUseCase _gameResourcesUseCase;
            private readonly RegisterUserUseCase _registerUserUseCase;
            private readonly ProfileUseCase _profileUseCase;

            public Controller(ProfileCreationView view, IWindowHandler windowHandler,
                GameResourcesUseCase gameResourcesUseCase, RegisterUserUseCase registerUserUseCase,
                ProfileUseCase profileUseCase)
                : base(view, windowHandler)
            {
                _windowHandler = windowHandler;
                _gameResourcesUseCase = gameResourcesUseCase;
                _registerUserUseCase = registerUserUseCase;
                _profileUseCase = profileUseCase;
            }

            public override async void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                AnimationHelper.ShowPopup_Scale(ConcreteView.transform);

                _nftCollection = openData as WalletNftCollectionDto;
                var urls = _nftCollection.Nfts.Select(x => x.ImageUrl).ToArray();

                WindowHandler.OpenWindow(WindowType.LoadingScreen);
                await UniTask.WaitForEndOfFrame();

                _gameResourcesUseCase.LoadCardSprites(urls, () =>
                {
                    _avatarDatas = new AvatarData[_nftCollection.Nfts.Length];
                    for (int i = 0; i < _nftCollection.Nfts.Length; i++)
                    {
                        var imageUrl = _nftCollection.Nfts[i].ImageUrl;
                        _avatarDatas[i] = new AvatarData()
                        {
                            ImageUrl = imageUrl,
                            SpriteValue = _gameResourcesUseCase.GetCardSpriteByUrl(imageUrl)
                        };
                    }

                    WindowHandler.CloseWindow(WindowType.LoadingScreen);
                });
            }

            void ButtonClick.ISubscribed.OnEvent(ButtonType buttonType)
            {
                switch (buttonType)
                {
                    case ButtonType.Create:
                        OnCreateProfileButtonHandler();
                        break;
                    case ButtonType.Avatar:
                        OnAvatarButtonHandler();
                        break;
                    case ButtonType.AvatarsClose:
                        ConcreteView.HideAvatarsScroll();
                        break;
                }
            }

            void TextInput.ISubscribed.OnEvent(InputType inputType, string value)
            {
                switch (inputType)
                {
                    case InputType.NickName:
                        _enteredNickName = ProfileCreationValidator.RemoveWhitespaceFromValue(value);
                        break;
                    case InputType.Bio:
                        _enteredBio = value;
                        break;
                }
            }

            void AvatarVariantButtonClick.ISubscribed.OnEvent(ButtonType btnType, string avatarId)
            {
                //TODO: save as url, not as id.
                _selectedAvatar = avatarId;
                var avatarSprite = _gameResourcesUseCase.GetCardSpriteByUrl(avatarId);
                ConcreteView.SetAvatarSprite(avatarSprite);
            }

            private void OnAvatarButtonHandler()
            {
                ConcreteView.ShowAvatarsScroll(_avatarDatas);
            }

            private void OnCreateProfileButtonHandler()
            {
                ProfileCreationValidator.ValidateProfileProperties(_enteredNickName, _enteredBio, _selectedAvatar,
                    (success, errorMessage) =>
                    {
                        if (success)
                            Register();
                        else
                            Debug.Log($"Profile data incorrect : " + errorMessage);
                    });
            }

            private void Register()
            {
                WindowHandler.OpenWindow(WindowType.LoadingScreen);
                _registerUserUseCase.Register(_enteredNickName, _selectedAvatar, _enteredBio,
                    onProfile: HandleProfileReceived);

                void HandleProfileReceived(ProfileReceivedEvent @event)
                {
                    Close();
                    WindowHandler.CloseWindow(WindowType.LoadingScreen);
                    _windowHandler.OpenWindow(WindowType.MainMenuCentral, @event.Profile);
                }
            }
        }

        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<ButtonClick>()
                .BindEvent<TextInput>()
                .BindEvent<AvatarVariantButtonClick>();
        }

        public class ButtonClick : EventHub<ButtonClick, ButtonType>
        {
        }

        public class TextInput : EventHub<TextInput, InputType, string>
        {
        }

        public class AvatarVariantButtonClick : EventHub<AvatarVariantButtonClick, ButtonType, string>
        {
        }

        public enum ButtonType
        {
            Create,
            Avatar,
            AvatarVariant,
            AvatarsClose,
        }

        public enum InputType
        {
            NickName,
            Bio
        }
    }
}