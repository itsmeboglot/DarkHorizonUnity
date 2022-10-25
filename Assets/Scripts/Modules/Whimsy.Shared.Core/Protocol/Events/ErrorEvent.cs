namespace Whimsy.Shared.Core.Protocol.Events
{
    public class ErrorEvent : IResponseEvent
    {
        public ErrorEvent()
        {
        }
    
        public ErrorType Type;
        public int Code;
        public string Message;
    }
}