using Darkhorizon.Shared.Player.Dto;
using Whimsy.Shared.Core;

namespace Gateways.Interfaces
{
    public class WalletCollectionReceivedEvent : IResponseEvent
    {
        public WalletNftCollectionDto WalletCollection;
    }
}