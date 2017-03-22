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
        Task<List<Device>> GetDevicesAsync(Guid userId, CancellationToken cancellationToken);

        Task AddDeviceAsync(Device device, CancellationToken cancellationToken);

        Task<Device> GetDeviceAsync(Guid userId, string imei, CancellationToken cancellationToken);

        Task<Device> GetDeviceByIdAsync(Guid userId, Guid deviceId, CancellationToken cancellationToken);

        Task<Device> GetDeviceAsync(string imei, CancellationToken cancellationToken);

        Task RemoveDeviceAsync(Device device, CancellationToken cancellationToken);
    }
}
