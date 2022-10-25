using System;
using UiScenario;
using Zenject;

namespace Core.UiScenario.Interface
{
    public interface IPresenter
    {
        void Install(DiContainer container, IWindowView windowView);
        Type ControllerType { get; }
    }
}