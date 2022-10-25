using System.Collections.Generic;
using System.Linq;
using Core.UiScenario.Concrete;
using Core.UiScenario.Data;
using Core.UiScenario.Interface;
using UiScenario;
using UnityEngine;

namespace Core.UiScenario
{
    public class WindowBuilder
    {
        private readonly IWindowControllerFactory _controllerFactory;
        private readonly WindowCanvasBuilder _canvasBuilder;
        private readonly WindowInfrastructure _windowInfrastructure;
        private readonly SetCanvasBounds _setCanvasBounds;

        private readonly Dictionary<WindowType, List<IWindowController>> _controllerPool =
            new Dictionary<WindowType, List<IWindowController>>();

        public WindowBuilder(SetCanvasBounds setCanvasBounds, IWindowControllerFactory windowControllerFactory,
            WindowCanvasBuilder canvasBuilder, WindowInfrastructure windowInfrastructure)
        {
            _controllerFactory = windowControllerFactory;
            _canvasBuilder = canvasBuilder;
            _windowInfrastructure = windowInfrastructure;
            _setCanvasBounds = setCanvasBounds;
        }

        public IWindowController Create(WindowType type, int topWindowOrder)
        {
            var controllers = GetControllerArray(type);
            var isExisted = controllers.Count > 0;
            var controller = isExisted ? controllers.First() : _controllerFactory.CreateWindowController(type);
            var view = controller.View;
            var canvasConfig = view.CanvasConfig;
            var order = view.Attributes.Contains(WindowAttribute.IgnoreSort)
                ? canvasConfig.OrderRange
                : topWindowOrder + canvasConfig.OrderRange;

            var asTransient = view.Attributes.Contains(WindowAttribute.AsTransient);
            if (!isExisted)
            {
                if (asTransient)
                    controller.Closed += windowController => GetControllerArray(type).Add(controller);
                else
                    GetControllerArray(type).Add(controller);
                var canvas = _canvasBuilder.Create(order, canvasConfig);
                var canvasTransform = canvas.GetComponent<RectTransform>();
                view.Transform.SetParent(canvasTransform, false);
                //It is the view of adaptation for devices like IPhoneX
                if (view.Attributes.Contains(WindowAttribute.IphoneXAdapt))
                    _setCanvasBounds.Add(view.Transform);

                view.SetData(_windowInfrastructure, canvas);
                view.Canvas.sortingOrder = order;
                view.Initialize();
                controller.Init();
            }
            else
            {
                view.Canvas.sortingOrder = order;
                if (asTransient)
                    GetControllerArray(type).Remove(controller);
            }

            view.Canvas.gameObject.SetActive(true);
            view.OnPop();
            return controller;
        }

        private List<IWindowController> GetControllerArray(WindowType type)
        {
            if (_controllerPool.ContainsKey(type))
                return _controllerPool[type];

            var list = new List<IWindowController>(1);
            _controllerPool.Add(type, list);
            return list;
        }
    }
}