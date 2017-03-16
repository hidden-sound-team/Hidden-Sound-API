using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Identity;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetDevicesAsync(HiddenSoundUser user, CancellationToken cancellationToken);

        Task AddDeviceAsync(Device device, CancellationToken cancellationToken);

        Task<Device> GetDeviceAsync(HiddenSoundUser user, string imei, CancellationToken cancellationToken);

        Task<Device> GetDeviceAsync(string imei, CancellationToken cancellationToken);
    }
}
