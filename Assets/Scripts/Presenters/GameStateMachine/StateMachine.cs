using System;
using System.Collections.Generic;
using Darkhorizon.Shared.Party.Protocol.Events;

using Whimsy.Shared.Core;

namespace Gateways.Interfaces
{
    public enum GameState
    {
        Idle,
        WaitForOpponentTurn,
        BattleStarted,
        BattleFinished,
        Attack,
        Defence,
        SelectStat,
        EndOfTurn,
        AutoAttack,
        AutoDefend,
        AutoSelectStat,
        OpponentReplenishTime
    }

    public class GameStateMachine : StateMachine<IResponseEvent, GameState>
    {
        private Dictionary<Type, GameState> @switch = new Dictionary<Type, GameState> 
        {
            { typeof(BattleStartedEvent),        GameState.BattleStarted },
            { typeof(BattleFinishedEvent),       GameState.BattleFinished },
            { typeof(YourAttackEvent),           GameState.Attack },
            { typeof(YourDefendEvent),           GameState.Defence },
            { typeof(YourSelectStatEvent),       GameState.SelectStat },
            { typeof(OtherAttackEvent),          GameState.WaitForOpponentTurn },
            { typeof(OtherDefendEvent),          GameState.WaitForOpponentTurn },
            { typeof(RoundEndEvent),             GameState.EndOfTurn },
            { typeof(YourAutoAttackEvent),       GameState.AutoAttack },
            { typeof(YourAutoDefendEvent),       GameState.AutoDefend },
            { typeof(YourAutoSelectStatEvent),   GameState.AutoSelectStat },
            { typeof(TimeReplenishedEvent),      GameState.OpponentReplenishTime },
            { typeof(OtherSelectStatEvent),      GameState.WaitForOpponentTurn },
        };
        
        public GameStateMachine ()
        {
            CurrentState = GameState.Idle;
        }

        public override void SetState(IResponseEvent state)
        {
            ObjData = state;
            CurrentState = @switch[state.GetType()];
        }
    }

    public abstract class StateMachine <T, S> 
        where T : IResponseEvent
        where S : Enum
    {
        protected T ObjData;
        public S CurrentState
        {
            get => _currentState;
            protected set
            {
                if (!_currentState.Equals(value))
                {
                    _currentState = value;
                    OnStateChanged?.Invoke(_currentState, ObjData);
                }
            }
        }
        
        public event Action<S, object> OnStateChanged;
        private S _currentState;

        public abstract void SetState(T state);

        public void SetState(S state)
        {
            CurrentState = state;
        }
    }
}