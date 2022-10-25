using Core.UiScenario.Concrete;
using Core.UiScenario.Interface;

namespace UiScenario
{
    public interface IWindowControllerFactory
    {
        IWindowController CreateWindowController(WindowType type);
    }
}