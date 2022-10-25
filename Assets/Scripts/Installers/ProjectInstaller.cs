using Game.Audio;
using Gateways.Connection;
using UnityEngine;
using Utils;
using Utils.Metamask;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [Header("Services")]
        [SerializeField] private MetamaskService metamaskServicePrefab;
        [SerializeField] private AudioManager audioManagerPrefab;
        
        [Header("Server URIs")]
        [SerializeField] private ServerUris serverUris;
        
        public override void InstallBindings()
        {
            Container.BindInstance(this).AsSingle();
            
            Container.InstallUseCases();
            Container.InstallServersGateways(serverUris);
            Container.InstallRepositories();
            Container.InstallMetamask(metamaskServicePrefab, transform);
            Container.InstallAudioPlayer(audioManagerPrefab, transform);
            
            var mono = gameObject.AddComponent<MainMono>();
            Container.BindInstance(mono).AsSingle();
        }
    }
}