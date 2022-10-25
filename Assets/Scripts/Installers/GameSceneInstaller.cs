using Presenters.Cards;
using Presenters.UI;
using Unity.SceneLoaders;
using Zenject;

namespace Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameSceneLoader>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LoadingScreenPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BattleResultsPresenter>().AsSingle().NonLazy();
                //Container.BindInterfacesAndSelfTo<CardsPresenter>().AsSingle().NonLazy();

        }
    }
}
