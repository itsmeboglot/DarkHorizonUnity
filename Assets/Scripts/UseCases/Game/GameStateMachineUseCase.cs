using System;
using Cysharp.Threading.Tasks;
using Darkhorizon.Shared.Party.Protocol.Events;
using Gateways.Interfaces;
using Zenject;

namespace UseCases.Game
{
    public class GameStateMachineUseCase : IDisposable
    {
        private readonly ILobbyGateway _lobbyGateway;
        private readonly GameStateMachine _stateMachine;

        public GameStateMachineUseCase (ILobbyGateway lobbyGateway)
        {
            _lobbyGateway = lobbyGateway;
            _stateMachine = new GameStateMachine();
        }

        void IDisposable.Dispose()
        {
            UnsubscribeFromGameEvents();
        }

        public void StartGameStateMachine()
        {
            SubscribeOnGameEvents();
        }

        public void SwitchToWaitingState()
        {
            _stateMachine.SetState(GameState.WaitForOpponentTurn);
        }

        public void SubscribeOnStateMachine (Action<GameState, object> callback)
        {
            _stateMachine.OnStateChanged += callback;
        }
        
        public void UnsubscribeFromStateMachine (Action<GameState, object> callback)
        {
            _stateMachine.OnStateChanged -= callback;
        }

        private void SubscribeOnGameEvents()
        {
            _lobbyGateway.EventHub.Subscribe<BattleStartedEvent>(BattleStartedEventHandler);
            _lobbyGateway.EventHub.Subscribe<BattleFinishedEvent>(BattleFinishedEventHandler);
            _lobbyGateway.EventHub.Subscribe<RoundEndEvent>(RoundEndEventHandler);
            _lobbyGateway.EventHub.Subscribe<YourAttackEvent>(YourAttackEventHandler);
            _lobbyGateway.EventHub.Subscribe<YourDefendEvent>(YourDefendEventHandler);
            _lobbyGateway.EventHub.Subscribe<YourSelectStatEvent>(YourSelectStatEventHandler);
            _lobbyGateway.EventHub.Subscribe<OtherAttackEvent>(OtherAttackEventHandler);
            _lobbyGateway.EventHub.Subscribe<OtherDefendEvent>(OtherDefendEventHandler);
            
            _lobbyGateway.EventHub.Subscribe<OtherSelectStatEvent>(OtherSelectStatEventHandler);
            _lobbyGateway.EventHub.Subscribe<TimeReplenishedEvent>(TimeReplenishedEventHandler);
            _lobbyGateway.EventHub.Subscribe<YourAutoAttackEvent>(YourAutoAttackEventHandler);
            _lobbyGateway.EventHub.Subscribe<YourAutoDefendEvent>(YourAutoDefendEventHandler);
            _lobbyGateway.EventHub.Subscribe<YourAutoSelectStatEvent>(YourAutoSelectStatEventHandler);
        }
        private void UnsubscribeFromGameEvents()
        {
            _lobbyGateway.EventHub.Unsubscribe<BattleStartedEvent>(BattleStartedEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<BattleFinishedEvent>(BattleFinishedEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<RoundEndEvent>(RoundEndEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<YourAttackEvent>(YourAttackEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<YourDefendEvent>(YourDefendEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<YourSelectStatEvent>(YourSelectStatEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<OtherAttackEvent>(OtherAttackEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<OtherDefendEvent>(OtherDefendEventHandler);
            
            _lobbyGateway.EventHub.Unsubscribe<OtherSelectStatEvent>(OtherSelectStatEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<TimeReplenishedEvent>(TimeReplenishedEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<YourAutoAttackEvent>(YourAutoAttackEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<YourAutoDefendEvent>(YourAutoDefendEventHandler);
            _lobbyGateway.EventHub.Unsubscribe<YourAutoSelectStatEvent>(YourAutoSelectStatEventHandler);
        }
        private void BattleStartedEventHandler(BattleStartedEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private async void BattleFinishedEventHandler(BattleFinishedEvent @event)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(5));
            _stateMachine.SetState(@event);
        }
        private void RoundEndEventHandler(RoundEndEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private void YourAttackEventHandler(YourAttackEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private void YourDefendEventHandler(YourDefendEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private void YourSelectStatEventHandler(YourSelectStatEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private void OtherAttackEventHandler(OtherAttackEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private void OtherDefendEventHandler(OtherDefendEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private void OtherSelectStatEventHandler(OtherSelectStatEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private void TimeReplenishedEventHandler(TimeReplenishedEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private void YourAutoAttackEventHandler(YourAutoAttackEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private void YourAutoDefendEventHandler(YourAutoDefendEvent @event)
        {
            _stateMachine.SetState(@event);
        }
        private void YourAutoSelectStatEventHandler(YourAutoSelectStatEvent @event)
        {
            _stateMachine.SetState(@event);
        }
      
    }
}