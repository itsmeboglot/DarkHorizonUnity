using Core.UiScenario;
using Core.UiScenario.Interface;
using Darkhorizon.Shared.Party.Protocol.Events;
using Gateways.Interfaces;
using UseCases.Game;
using Utils.Logger;
using Views.UI;

namespace Presenters.UI
{
    public class TimerPresenter : Presenter
    {
        private class Controller : Controller<TimerView>
        {
            private readonly GameStateMachineUseCase _stateMachineUseCase;
            private readonly GameCommandsUseCase _gameCommandsUseCase;

            public Controller(TimerView view, IWindowHandler windowHandler, GameStateMachineUseCase stateMachineUseCase, 
                GameCommandsUseCase gameCommandsUseCase) : base(view, windowHandler)
            {
                _stateMachineUseCase = stateMachineUseCase;
                _gameCommandsUseCase = gameCommandsUseCase;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                _stateMachineUseCase.SubscribeOnStateMachine(HandleState);
            }

            private void HandleState(GameState gameState, object data)
            {
                switch (gameState)
                {
                    case GameState.WaitForOpponentTurn:
                        ConcreteView.SetOpponentTurnColor();
                        ConcreteView.AnimateTimer(30, "WAITING FOR OPPONENT!");
                        break;
                    case GameState.Attack:
                        ConcreteView.SetYourTurnColor();
                        ConcreteView.AnimateTimer(((YourAttackEvent) data).TimeLeft, "YOUR TURN TO ATTACK!");
                        break;
                    case GameState.Defence:
                        ConcreteView.SetYourTurnColor();
                        ConcreteView.AnimateTimer(((YourDefendEvent) data).TimeLeft, "YOUR TURN TO DEFENCE!");
                        break;
                    case GameState.SelectStat:
                        ConcreteView.SetYourTurnColor();
                        ConcreteView.AnimateTimer(((YourSelectStatEvent) data).TimeLeft, "WAITING FOR OPPONENT!");
                        break;
                    case GameState.OpponentReplenishTime:
                        CustomLogger.Log(LogSource.Unity, $"Replenish Time!!");
                        // var newTime = ((TimeReplenishedEvent) data).TimeLeft + ConcreteView.SecondsLeft;
                        ConcreteView.AnimateTimer(((TimeReplenishedEvent) data).TimeLeft, "EXTRA TIME WAS USED!");
                        break;
                    case GameState.EndOfTurn:
                        ConcreteView.HideTimer();
                        break;
                }
            }
        }

        protected override void InstallBindings()
        {
            BindController<Controller>();
        }
        
    }
}