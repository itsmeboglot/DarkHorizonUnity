using Darkhorizon.Client.ApiHub;
using Gateways.EventHub;
using Whimsy.Shared.Identity;

namespace Gateways.Interfaces
{
    public interface IHttpApiGateway
    {
        void SetCurrentUser(UserToken token);
        UserToken CurrentUser { get; }
        IDarkhorizonServerApiHub ApiHub { get; }
        IWalletCollectionServerApi WalletApi { get; }
        IServerEventHub EventHub { get; }
    }
}