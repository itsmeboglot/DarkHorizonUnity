using Core.UiScenario;
using Core.UiScenario.Interface;
using UseCases.Menu;
using Views.UI;

namespace Presenters.UI
{
    public class BackgroundPresenter : Presenter
    {
        private class Controller : Controller<BackgroundView>
        {
            private readonly ProjectInfoUseCase _projectInfoUseCase;

            public Controller(BackgroundView view, IWindowHandler windowHandler,
                ProjectInfoUseCase projectInfoUseCase) : base(view, windowHandler)
            {
                _projectInfoUseCase = projectInfoUseCase;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                ConcreteView.SetData(_projectInfoUseCase.GetProjectVersion(), _projectInfoUseCase.GetEnvironment());
            }
        }
        
        protected override void InstallBindings()
        {
            BindController<Controller>();
        }
    }
}