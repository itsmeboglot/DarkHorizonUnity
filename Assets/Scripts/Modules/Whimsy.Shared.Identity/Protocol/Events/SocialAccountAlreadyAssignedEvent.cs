using Whimsy.Shared.Core;

namespace Whimsy.Shared.Identity.Protocol.Events
{
    public class SocialAccountAlreadyAssignedEvent : IResponseEvent
    {
        public SocialAccountAlreadyAssignedEvent()
        {
            
        }
        
        public UserToken AssignedToken;
    }
}