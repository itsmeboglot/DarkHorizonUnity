using System;
using Darkhorizon.Shared.Player.Dto;
using Darkhorizon.Shared.Player.Protocol.Events;

namespace Gateways.Interfaces
{
    public interface IProfileRepository
    {
        event Action<ProfileDto> OnProfileUpdated;
        ProfileDto GetProfile();
        void LoadProfile(Action<ProfileReceivedEvent> onSucceed, Action<NeedRegistrationEvent> onFail = null);
    }
}