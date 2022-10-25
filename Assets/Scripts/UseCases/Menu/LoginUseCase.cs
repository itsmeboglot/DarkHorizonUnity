using System;
using Gateways.Interfaces;
using UnityEngine;
using Whimsy.Client.Core;
using Whimsy.Shared.Identity;
using Whimsy.Shared.Identity.Protocol.Events;

namespace UseCases.Menu
{
    public class LoginUseCase
    {
        private readonly IHttpApiGateway _httpApiGateway;

        public LoginUseCase(IHttpApiGateway httpApiGateway)
        {
            _httpApiGateway = httpApiGateway;
        }

        public void LoginByMetaMask(string secret, string message, Action<SendingResult> resultCallback = null)
        {
            _httpApiGateway.ApiHub.MetamaskIdentity.Login(secret, message, resultCallback);
        }

        public void Subscribe(Action<LoggedInEvent> doNext)
        {
            _httpApiGateway.EventHub.Subscribe(doNext);
        }
        
        public void Unsubscribe(Action<LoggedInEvent> doNext)
        {
            _httpApiGateway.EventHub.Unsubscribe(doNext);
        }

        public void SetUser(UserToken userToken)
        {
            _httpApiGateway.SetCurrentUser(userToken);
        }
    }
}