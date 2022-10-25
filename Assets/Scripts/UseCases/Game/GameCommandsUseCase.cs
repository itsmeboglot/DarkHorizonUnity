using System;
using System.Collections.Generic;
using Darkhorizon.Shared.Dto;
using Darkhorizon.Shared.Party.Protocol.Commands.Battle;
using Entities.Card;
using Gateways.Interfaces;

namespace UseCases.Game
{
    public class GameCommandsUseCase
    {
        public Action<int> OnBoosterUsed;
        public int[] LastUsedBoosterIds => _lastUsedBoosterIds.ToArray();

        private readonly List<int> _lastUsedBoosterIds = new List<int>();
        private readonly ILobbyGateway _lobbyGateway;

        public GameCommandsUseCase(ILobbyGateway lobbyGateway)
        {
            _lobbyGateway = lobbyGateway;
        }
        
        public void SendMyAttack(int cardId, CardStatsType statType)
        {
            StatTypeDto statTypeDto = (StatTypeDto) statType;
            var attackCommand = new AttackCommand
            {
                CardId = cardId,
                StatType = statTypeDto,
                BoostersIds = _lastUsedBoosterIds.ToArray()
            };
 
            _lobbyGateway.Send(attackCommand);
        }
        
        public void SendMyDefence(int cardId)
        {
            _lobbyGateway.Send(new DefendCommand
            {
                CardId = cardId,
                BoostersIds = LastUsedBoosterIds
            });
        }
        
        public void SendSelectedStat(CardStatsType statType)
        {
            _lobbyGateway.Send(new SelectStatCommand
            {
                StatType = (StatTypeDto) statType
            });
        }
        
        public void SendReplenishTime ()
        {
            _lobbyGateway.Send(new ReplenishTimeCommand());
        }
        
        public void SendBoosterUsed(int boosterId)
        {
            OnBoosterUsed?.Invoke(boosterId);
            _lastUsedBoosterIds.Add(boosterId);
        }

        /// <summary>
        /// Call after showdown
        /// </summary>
        public void ClearLastUsedBoosters()
        {
            _lastUsedBoosterIds.Clear();
        }
    }
}