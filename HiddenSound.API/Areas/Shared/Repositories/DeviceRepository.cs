using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Identity;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        public HiddenSoundDbContext HiddenSoundDbContext { get; set; }

        public List<Device> GetDevices(HiddenSoundUser user)
        {
            return HiddenSoundDbContext.Devices.Where(d => d.UserId == user.Id).ToList();
        }

        public bool HasDevice(HiddenSoundUser user, string imei)
        {
            return
                HiddenSoundDbContext.Devices.Any(
                    d => d.UserId == user.Id && string.Equals(d.IMEI, imei, StringComparison.OrdinalIgnoreCase));
        }
    }
}
