using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HiddenSound.API.OpenIddict
{
    public class ApplicationRepository : IApplicationRepository
    {
        public HiddenSoundDbContext HiddenSoundDbContext { get; set; }

        public Task<List<HSOpenIddictApplication>> GetApplicationsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return HiddenSoundDbContext.Applications.Where(a => a.UserId == userId).ToListAsync(cancellationToken);
        }

        public Task<HSOpenIddictApplication> GetApplicationByClientIdAsync(Guid userId, string clientId, CancellationToken cancellationToken)
        {
            return HiddenSoundDbContext.Applications.FirstOrDefaultAsync(a => a.UserId == userId && a.ClientId == clientId, cancellationToken);
        }
    }
}
