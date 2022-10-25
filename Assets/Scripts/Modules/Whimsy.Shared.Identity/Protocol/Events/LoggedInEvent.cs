using System;
using Whimsy.Shared.Core;

namespace Whimsy.Shared.Identity.Protocol.Events
{
    public class LoggedInEvent : IResponseEvent
    {
        public LoggedInEvent()
        {
            
        }
        
        public UserToken Token;
    }
}