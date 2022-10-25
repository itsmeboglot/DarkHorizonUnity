using Whimsy.Shared.Core;

namespace Whimsy.Client.Core
{
    public interface IProtocolFeature
    {
        void Initialize(IServerEventSource serverEventSource);
        void EnrichHeader(IRequestCommand command, RequestHeader header);
    }
}