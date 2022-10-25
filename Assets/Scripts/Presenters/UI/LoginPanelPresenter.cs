using System;
using System.Collections.Generic;
using System.Linq;
using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using Darkhorizon.Shared.Player.Protocol.Events;
using UseCases.Addressables;
using UseCases.Menu;
using Utils.Logger;
using Views.UI;
using Whimsy.Client.Core;
using Whimsy.Shared.Identity.Protocol.Events;

namespace Presenters.UI
{
    public class LoginPanelPresenter : Presenter
    {
        private class Controller : Controller<LoginPanelView>, ButtonClick.ISubscribed
        {
            private readonly ObserveMetaMaskConnectUseCase _metaMaskConnectUseCase;
            private readonly ProfileUseCase _profileUseCase;
            private readonly GameResourcesUseCase _gameResourcesUseCase;
            private readonly LoginUseCase _loginUseCase;

            public Controller(LoginPanelView view, IWindowHandler windowHandler,
                ObserveMetaMaskConnectUseCase metaMaskConnectUseCase, ProfileUseCase profileUseCase,
                GameResourcesUseCase gameResourcesUseCase, LoginUseCase loginUseCase)
                : base(view, windowHandler)
            {
                _metaMaskConnectUseCase = metaMaskConnectUseCase;
                _profileUseCase = profileUseCase;
                _gameResourcesUseCase = gameResourcesUseCase;
                _loginUseCase = loginUseCase;
            }

            void ButtonClick.ISubscribed.OnEvent(ButtonType buttonType)
            {
                switch (buttonType)
                {
                    case ButtonType.MetaMask:
                        WindowHandler.OpenWindow(WindowType.LoadingScreen);
                        _metaMaskConnectUseCase.Connect(HandleMetaMaskComplete);
                        break;
                }
            }

            private void HandleMetaMaskComplete(string secret, string account, string message)
            {
                _metaMaskConnectUseCase.DisposeSubscribe(HandleMetaMaskComplete);

                _loginUseCase.Subscribe(HandleLoggedIn);
                _loginUseCase.LoginByMetaMask(secret, message, result =>
                {
                    if (result == SendingResult.Failed)
                    {
                        WindowHandler.CloseWindow(WindowType.LoadingScreen);
                        CustomLogger.Log(LogSource.Server, "Login failed!", MessageType.Error);
                        _loginUseCase.Unsubscribe(HandleLoggedIn);
                    }
                });
            }

            private void HandleLoggedIn(LoggedInEvent userToken)
            {
                _loginUseCase.Unsubscribe(HandleLoggedIn);
                CustomLogger.Log(LogSource.Server, $"Login succeed! ID:{userToken.Token.UserId}");
                _loginUseCase.SetUser(userToken.Token);
                GetProfile();
            }

            private void GetProfile()
            {
                _profileUseCase.LoadProfile(HandleProfileReceived, HandleNeedRegistration);

                void HandleProfileReceived(ProfileReceivedEvent @event)
                {
                    var urls = @event.Profile.CharacterCards.Select(x => x.ImageUrl).ToList();
                    urls.Add(@event.Profile.PersonalInfo.Avatar);
                    LoadAvatars(urls, () =>
                    {
                        Close();
                        WindowHandler.CloseWindow(WindowType.LoadingScreen);
                        WindowHandler.OpenWindow(WindowType.MainMenuCentral, @event.Profile);
                    });
                }

                void HandleNeedRegistration(NeedRegistrationEvent @event)
                {
                    var urls = @event.WalletNftCollection.Nfts.Select(x => x.ImageUrl).ToList();
                    
                    LoadAvatars(urls, () =>
                    {
                        Close();
                        WindowHandler.CloseWindow(WindowType.LoadingScreen);
                        WindowHandler.OpenWindow(WindowType.ProfileCreation, @event.WalletNftCollection);
                    });
                }

                void LoadAvatars(IEnumerable<string> urls, Action doNext)
                {
                    _gameResourcesUseCase.LoadCardSprites(urls, doNext);
                }
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
            MetaMask
        }
    }
}