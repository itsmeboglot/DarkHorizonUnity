using System;
using Core.UiScenario.Concrete;

namespace Core.UiScenario.Interface
{
    public interface IWindowHandler
    {
        event Action<WindowType> OpenWindowEvent;
        event Action<WindowType> CloseWindowEvent;
        bool IsOpen(WindowType type);
        void OpenWindow(WindowType type, object openData = null);
        void CloseWindow(WindowType type);
        IWindowController GetTopWindow();
    }
}