using Gateways.Interfaces;
using UnityEngine;

namespace UseCases.Menu
{
    public class ProjectInfoUseCase
    {
        private const string Prod = "5020";
        private const string Dev = "5030";
        private readonly ILobbyGateway _lobbyGateway;
        
        public ProjectInfoUseCase (ILobbyGateway lobbyGateway)
        {
            _lobbyGateway = lobbyGateway;
        }
        
        public string GetProjectVersion()
        {
            return $"v {Application.version}";
        }

        public string GetEnvironment()
        {
            var port = _lobbyGateway.GetPort();
            return port.Equals(Prod) ? nameof(Prod) : nameof(Dev);
        }
    }
}