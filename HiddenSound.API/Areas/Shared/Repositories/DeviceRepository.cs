using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoHelper;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Identity;
using Microsoft.EntityFrameworkCore;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        public HiddenSoundDbContext HiddenSoundDbContext { get; set; }

        public Task<List<Device>> GetDevicesAsync(HiddenSoundUser user, CancellationToken cancellationToken)
        {
            return HiddenSoundDbContext.Devices.Where(d => d.UserId == user.Id).ToListAsync(cancellationToken);
        }

        public Task AddDeviceAsync(Device device, CancellationToken cancellationToken)
        {
            HiddenSoundDbContext.Devices.Add(device);
            return HiddenSoundDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<Device> GetDeviceAsync(HiddenSoundUser user, string imei, CancellationToken cancellationToken)
        {
            return HiddenSoundDbContext.Devices.FirstOrDefaultAsync(d => d.UserId == user.Id && Crypto.VerifyHashedPassword(d.IMEI, imei), cancellationToken);
        }

        public Task<Device> GetDeviceAsync(string imei, CancellationToken cancellationToken)
        {
            return HiddenSoundDbContext.Devices.FirstOrDefaultAsync(d => Crypto.VerifyHashedPassword(d.IMEI, imei), cancellationToken);
        }
    }
}
