using System;

namespace Gateways.Interfaces
{
    public interface ILobbyGateway : ISocketGateway
    {
        public event Action OnSocketOpened;
        public string GetPort();
    }
}