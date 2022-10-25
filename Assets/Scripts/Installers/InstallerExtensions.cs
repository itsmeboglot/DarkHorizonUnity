using BestHTTP;
using Game.Audio;
using Gateways;
using Gateways.Addressables;
using Gateways.Connection;
using UnityEngine;
using UseCases.Addressables;
using UseCases.Common;
using UseCases.Game;
using UseCases.Menu;
using Utils.Metamask;
using Zenject;

namespace Installers
{
    public static class InstallerExtensions
    {
        public static void InstallUseCases(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<ErrorListenerUseCase>().AsSingle().NonLazy();
            container.BindInterfacesAndSelfTo<NotificationListenerUseCase>().AsSingle();
            container.BindInterfacesAndSelfTo<GameStateMachineUseCase>().AsSingle();
            container.Bind<GameResourcesUseCase>().AsSingle();
            container.Bind<ObserveMetaMaskConnectUseCase>().AsSingle();
            container.Bind<ProfileUseCase>().AsSingle();
            container.Bind<LoginUseCase>().AsSingle();
            container.Bind<RegisterUserUseCase>().AsSingle();
            container.Bind<LobbyConnectionUseCase>().AsSingle();
            container.Bind<SearchGameUseCase>().AsSingle();
            container.Bind<GameplayUseCase>().AsSingle();
            container.Bind<GameEndUseCase>().AsSingle();
            container.Bind<GameCommandsUseCase>().AsSingle();
            container.Bind<DeckEditingUseCase>().AsSingle();
            container.Bind<ProjectInfoUseCase>().AsSingle();
            container.BindInterfacesAndSelfTo<CardInfoUseCase>().AsSingle();
        }
        
        public static void InstallServersGateways(this DiContainer container, ServerUris serverUris)
        {
            HTTPManager.Setup();

            container.BindInterfacesTo<LobbyGateway>().AsSingle().WithArguments(serverUris.socket.lobbySocketUri);
            container.BindInterfacesTo<HttpApiGateway>().AsSingle().WithArguments(serverUris.http);
            container.BindInterfacesTo<GameResourcesGateway>().AsSingle();
        }

        public static void InstallRepositories(this DiContainer container)
        {
            container.BindInterfacesTo<ProfileRepository>().AsSingle();
        }

        public static void InstallMetamask(this DiContainer container, MetamaskService prefab, Transform parent)
        {
            var metamaskService = Object.Instantiate(prefab, parent);
            metamaskService.name = metamaskService.name.Replace("(Clone)", string.Empty);
            container.BindInterfacesTo<MetamaskService>().FromComponentOn(metamaskService.gameObject).AsSingle().NonLazy();
        }
        
        public static void InstallAudioPlayer(this DiContainer container, AudioManager prefab, Transform parent)
        {
            var audioManager = Object.Instantiate(prefab, parent);
            container.BindInterfacesTo<AudioPlayerProvider>().AsSingle().WithArguments(audioManager);
        }
    }
}