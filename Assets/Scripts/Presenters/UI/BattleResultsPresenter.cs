using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Interface;
using UseCases.Menu;
using Views.UI;

namespace Presenters.UI
{
    public class BattleResultsPresenter : Presenter
    {
        private class Controller : Controller<BattleResultsView>, ButtonClick.ISubscribed
        {
            private readonly IWindowHandler _windowHandler;
            private readonly LobbyConnectionUseCase _lobbyConnectionUseCase;

            public Controller(BattleResultsView view, IWindowHandler windowHandler, LobbyConnectionUseCase lobbyConnectionUseCase) : base(view, windowHandler)
            {
                _windowHandler = windowHandler;
                _lobbyConnectionUseCase = lobbyConnectionUseCase;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                var data = (BattleResultsData) openData;
                
               
                ConcreteView.SetData(data);
                // (BattleFinishedEvent)openData
                //ConcreteView.SetData();
            }

            public void OnEvent()
            {
                _lobbyConnectionUseCase.ClearLobbySubscribes();
                _lobbyConnectionUseCase.Disconnect();
                Close();
                
            }
        }
        
        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<ButtonClick>();
        }

        public class ButtonClick : EventHub<ButtonClick>
        {
            
        }
        
    }
}