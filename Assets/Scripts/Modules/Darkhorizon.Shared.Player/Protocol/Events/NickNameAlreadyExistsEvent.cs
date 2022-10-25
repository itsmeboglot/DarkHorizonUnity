using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Player.Protocol.Events
{
    public class NickNameAlreadyExistsEvent : IResponseEvent, IResponseNotification
    {
        public static readonly NickNameAlreadyExistsEvent Instance = new NickNameAlreadyExistsEvent();
        
        private NickNameAlreadyExistsEvent()
        {
        }
    }
}