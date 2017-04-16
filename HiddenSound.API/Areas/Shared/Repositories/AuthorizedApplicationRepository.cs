using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public class AuthorizedApplicationRepository : IAuthorizedApplicationRepository
    {
        public HiddenSoundDbContext HiddenSoundDbContext { get; set; }

        public Task<List<AuthorizedApplication>> GetAuthorizedApplicationsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var list = HiddenSoundDbContext.AuthorizedApplications.Where(a => a.UserId == userId)
                .Join(HiddenSoundDbContext.Applications, a => a.ApplicationId, app => app.Id, (a, app) => new { AuthorizedApplication = a, App = app });

            foreach (var x in list)
            {
                x.AuthorizedApplication.Application = x.App;
            }

            return list.Select(a => a.AuthorizedApplication).ToListAsync(cancellationToken);
        }

        public Task<AuthorizedApplication> GetAuthorizedApplicationAsync(Guid userId, Guid applicationId, CancellationToken cancellationToken)
        {
            return HiddenSoundDbContext.AuthorizedApplications.FirstOrDefaultAsync(a => a.UserId == userId && a.ApplicationId == applicationId, cancellationToken);
        }

        public Task CreateAuthorizedApplicationAsync(AuthorizedApplication authorizedApplication, CancellationToken cancellationToken)
        {
            HiddenSoundDbContext.AuthorizedApplications.Add(authorizedApplication);
            return HiddenSoundDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task RemoveAuthorizedApplicationAsync(AuthorizedApplication authorizedApplication, CancellationToken cancellationToken)
        {
            HiddenSoundDbContext.AuthorizedApplications.Remove(authorizedApplication);
            return HiddenSoundDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
