using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Identity;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public interface IDeviceRepository
    {
        List<Device> GetDevices(HiddenSoundUser user);

        bool HasDevice(HiddenSoundUser user, string imei);
    }
}
