using System;
using Whimsy.Shared.Core;

namespace Whimsy.Shared.Identity.Protocol.Commands
{
    public class LoginByDeviceIdCommand : IGuestRequestCommand
    {
        public string DeviceId;
    }
}