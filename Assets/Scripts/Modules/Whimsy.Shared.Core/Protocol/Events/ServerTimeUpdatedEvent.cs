using System;

namespace Whimsy.Shared.Core.Protocol.Events
{
    public class ServerTimeUpdatedEvent : IResponseEvent
    {
        public ServerTimeUpdatedEvent()
        {
        }
        
        public int Time;
    }
}