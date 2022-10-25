using Gateways.EventHub;
using Whimsy.Shared.Core;

namespace Gateways.Interfaces
{
    public interface ISocketGateway
    {
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        void Send(IRequestCommand command);
        IServerEventHub EventHub { get; }
        void ClearSubscribes();
    }
}