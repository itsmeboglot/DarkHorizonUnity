using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Player.Protocol.Events
{
    public class RegisterSucceedEvent : IResponseEvent
    {
        public static readonly RegisterSucceedEvent Instance = new RegisterSucceedEvent();
        
        private RegisterSucceedEvent()
        {
            
        }
    }
}