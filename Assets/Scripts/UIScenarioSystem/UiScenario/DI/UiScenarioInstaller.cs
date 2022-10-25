using System;
using System.Collections.Generic;
using Core.EventAggregator;
using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;
using Core.UiScenario.Pool;
using Core.VBCM;
using Core.View.ViewPool;
using UiScenario;
using UnityEngine;
using Zenject;

namespace Core.UiScenario.DI
{
    public class UiScenarioInstaller : MonoInstaller<UiScenarioInstaller>, IWindowControllerFactory
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private Canvas defaultCanvas;
        [SerializeField] private ViewPoolable[] poolablePrefabs;
        [SerializeField] private ScenarioViewPoolObject[] windowPrefabs;
        private readonly Dictionary<WindowType, Type[]> _viewTypes = new Dictionary<WindowType, Type[]>();
        private readonly Dictionary<WindowType, Type> _controllerTypes = new Dictionary<WindowType, Type>();
        private WindowInfrastructure _windowInfrastructure;

        public override void InstallBindings()
        {
            Container.Bind<SetCanvasBounds>().AsSingle();
            Container.Bind<IWindowControllerFactory>().FromInstance(this);
            Container.Bind<WindowBuilder>().AsSingle();
            Container.Bind(typeof(IWindowHandler), typeof(IDisposable)).To<WindowHandler>().AsSingle();
            //Canvas===========================================
            Container.Bind<WindowCanvasConfigurator>().AsSingle();
            Container.Bind<WindowCanvasBuilder>().AsSingle().WithArguments(defaultCanvas);
            //========================================
            if (camera != null)
                Container.BindInstance(camera);

            VbcmInstaller.Install(Container);
            EventAggregatorInstaller.Install(Container);
            Container.Bind(typeof(ViewPool)).To<ViewPool>().AsSingle().WithArguments(poolablePrefabs);
            Container.Bind<ScenarioViewPool>().AsSingle().WithArguments(windowPrefabs);
            Container.Bind<WindowInfrastructure>().AsSingle();
            //========================================
            foreach (var view in windowPrefabs)
            {
                var windowView = (IWindowView) view;
                var (windowType, controllerType) = windowView.Install(Container);
                _controllerTypes.Add(windowType, controllerType);
            }

            Container.BindInstance(_viewTypes);
        }

        public IWindowController CreateWindowController(WindowType type)
        {
            if (!_controllerTypes.ContainsKey(type))
                Debug.LogError($"You have not bind controller: {type}");

            return (IWindowController) Container.Resolve(_controllerTypes[type]);
        }
    }
}