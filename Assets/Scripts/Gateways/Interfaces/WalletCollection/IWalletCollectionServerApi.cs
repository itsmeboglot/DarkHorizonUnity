using System;
using Whimsy.Client.Core;

namespace Gateways.Interfaces
{
    public interface IWalletCollectionServerApi
    {
         void GetWalletCollection(Action<SendingResult> onCompletedCallback = null);
    }
}