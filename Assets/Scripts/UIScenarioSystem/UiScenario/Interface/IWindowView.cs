using System;
using Core.UiScenario.Concrete;
using Core.UiScenario.Data;
using UiScenario;
using UnityEngine;
using Zenject;

namespace Core.UiScenario.Interface
{
    public interface IWindowView
    {
        Canvas Canvas { get; }
        WindowAttribute[] Attributes { get; }
        WindowCanvasConfig CanvasConfig { get; }
        RectTransform Transform { get; }
        void Block();
        void Unblock();
        void Initialize();
        (WindowType, Type) Install(DiContainer container);
        void Bind(DiContainer subContainer);
        void SetData(WindowInfrastructure windowInfrastructure, Canvas canvas);
        void OnPop();
        void OnPush();
    }
}