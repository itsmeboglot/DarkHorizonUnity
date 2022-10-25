using Darkhorizon.Shared.Party.Protocol.Commands;
using Gateways.Interfaces;

namespace UseCases.Game
{
    public class GameplayUseCase 
    {
        private readonly ILobbyGateway _gameGateway;

        public GameplayUseCase(ILobbyGateway gameGateway)
        {
            _gameGateway = gameGateway;
        }

        public void ReadyForParty ()
        {
            _gameGateway.Send(new ReadyForPartyCommand());
        }
   
    }
}