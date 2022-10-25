using System;
using System.Collections.Generic;
using System.Linq;
using Core.EventAggregator;
using Core.EventAggregator.Interface;
using Core.UiScenario.Concrete;
using Core.UiScenario.Data;
using Core.UiScenario.Interface;
using Core.UiScenario.Pool;
using UiScenario;
using UnityEngine;
using Zenject;

namespace Core.UiScenario
{
    public abstract class Presenter : IPresenter, IEventBinder
    {
        private static readonly Type ScenarioViewPoolType = typeof(ScenarioViewPool);

        public abstract class View<TContractor> : ScenarioViewPoolObject, IWindowView
            where TContractor : Presenter, new()
        {
            protected WindowInfrastructure Infrastructure { get; private set; }
            protected IBinder Binder { get; private set; }

            #region internal

            [SerializeField] private WindowAttribute[] _attributes;
            [SerializeField] private WindowCanvasConfig _canvasConfig;
            public WindowAttribute[] Attributes => _attributes;
            public WindowCanvasConfig CanvasConfig => _canvasConfig;

            void IWindowView.SetData(WindowInfrastructure windowInfrastructure, Canvas canvas)
            {
                Infrastructure = windowInfrastructure;
                Canvas = canvas;
            }

            (WindowType, Type) IWindowView.Install(DiContainer container)
            {
                IPresenter presenter = new TContractor();
                presenter.Install(container, this);
                return (Type, presenter.ControllerType);
            }

            void IWindowView.Bind(DiContainer subContainer)
            {
                subContainer.Bind(GetType()).FromMethodUntyped(context =>
                {
                    var pool = (ScenarioViewPool) context.Container.Resolve(ScenarioViewPoolType);
                    var view = (View<TContractor>) pool.Pop(Type);
                    view.Binder = new Binder(subContainer.Resolve<IEventAggregator>(), subContainer);
                    return view;
                }).AsSingle();
            }

            #endregion

            public virtual void Initialize()
            {
            }

            public virtual void Block()
            {
            }

            public virtual void Unblock()
            {
            }

           
        }

        public abstract class Controller<TView> : IWindowController
            where TView : ScenarioViewPoolObject, IWindowView
        {
            protected readonly TView ConcreteView;
            IWindowView IWindowController.View => ConcreteView;
            public IWindowHandler WindowHandler { get; }
            public WindowType Type { get; }

            private bool _animate;
            
            #region internal

            public event Action<IWindowController> Closed;

            protected Controller(TView view, IWindowHandler windowHandler)
            {
                ConcreteView = view;
                WindowHandler = windowHandler;
                Type = ConcreteView.Type;
            }

            #endregion

            public virtual void Init()
            {
            }

            public virtual void Open(object openData, bool animate = true)
            {
                _animate = animate;
                if (_animate)
                {
                    //DOTween.Sequence().Append(); ToDo:Do
                }
            }

            public virtual void OnClose()
            {
            }

            public virtual void Block()
            {
            }

            public virtual void Unblock()
            {
            }

            public void Close()
            {
                if (_animate)
                {
                    //ToDo:
                }
                ConcreteView.Push();
                Closed?.Invoke(this);
            }
        }

        protected abstract void InstallBindings();

        #region internal

        private DiContainer _container;
        private IWindowView _windowView;
        private readonly List<Type> _events = new List<Type>();
        private Type _controllerType;
        Type IPresenter.ControllerType => _controllerType;

        void IPresenter.Install(DiContainer container, IWindowView windowView)
        {
            _container = container;
            _windowView = windowView;
            InstallBindings();
            
            if (!_windowView.Attributes.Contains(WindowAttribute.AsTransient))
                InstallSubContainer(_container, _controllerType);
        }

        protected IEventBinder BindController<TController>(params Type[] controllerTypes)
            where TController : IWindowController
        {
            _controllerType = typeof(TController);

            if (_windowView.Attributes.Contains(WindowAttribute.AsTransient))
                _container.Bind(_controllerType).FromSubContainerResolve().ByMethod(subContainer =>
                    InstallSubContainer(subContainer, _controllerType)).AsTransient();
            
            return this;
        }

        private void InstallSubContainer(DiContainer subContainer, Type controllerType)
        {
            subContainer.BindInterfacesAndSelfTo(controllerType).AsSingle();
            _windowView.Bind(subContainer);
            
            foreach (var @event in _events)
                subContainer.Bind(@event).AsSingle();
        }

        IEventBinder IEventBinder.BindEvent<TEvent>()
        {
            _events.Add(typeof(TEvent));
            return this;
        }

        #endregion internal
    }
}