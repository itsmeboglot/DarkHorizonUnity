using System;
using Core.UiScenario.Concrete;
using UiScenario;

namespace Core.UiScenario.Interface
{
    public interface IWindowController
    {
        event Action<IWindowController> Closed;
        IWindowHandler WindowHandler { get; }
        WindowType Type { get; }
        IWindowView View { get; }
        void Init();
        void Open(object openData, bool autoAnimate = true);
        void Block();
        void Unblock();
        void Close();
        void OnClose();
    }
}