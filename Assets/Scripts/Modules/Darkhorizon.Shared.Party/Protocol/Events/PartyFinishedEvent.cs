using Whimsy.Shared.Core;

namespace Darkhorizon.Shared.Party.Protocol.Events
{
    public class PartyFinishedEvent : IResponseEvent
    {
        public static readonly PartyFinishedEvent Instance = new PartyFinishedEvent();

        #region Constructors

        private PartyFinishedEvent()
        {
        }

        #endregion
    }
}