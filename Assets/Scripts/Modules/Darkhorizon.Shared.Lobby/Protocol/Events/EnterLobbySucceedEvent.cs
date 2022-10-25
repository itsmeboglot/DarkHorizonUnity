using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Lobby.Protocol.Events
{
    public class EnterLobbySucceedEvent : IResponseEvent
    {
        public static readonly EnterLobbySucceedEvent Instance = new EnterLobbySucceedEvent();
        
        private EnterLobbySucceedEvent()
        {
            
        }
    }
}