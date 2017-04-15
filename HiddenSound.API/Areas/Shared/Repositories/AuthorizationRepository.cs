using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.OpenIddict;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        public HiddenSoundDbContext DbContext { get; set; }

        public Authorization GetAuthorization(string authorizationCode)
        {
            return DbContext.Authorizations.FirstOrDefault(a => a.Code == authorizationCode);
        }

        public Task<Authorization> GetAuthorizationAsync(Guid application, string code, CancellationToken cancellationToken)
        {
            return DbContext.Authorizations.FirstOrDefaultAsync(a => a.ApplicationId == application && a.Code == code, cancellationToken);
        }

        public void UpdateAuthorization(Authorization authorization)
        {
            DbContext.Authorizations.Update(authorization);
            DbContext.SaveChanges();
        }

        public void CreateAuthorization(Authorization authorization)
        {
            DbContext.Authorizations.Add(authorization);
            DbContext.SaveChanges();
        }

        public List<Authorization> GetAllAuthorizations()
        {
            return DbContext.Authorizations.ToList();
        }

        public Task RemoveAuthorizationsByApplicationAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            var authorizations = GetAuthorizationsByApplicationAsync(applicationId, cancellationToken).GetAwaiter().GetResult();
 
            DbContext.Authorizations.RemoveRange(authorizations);
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<List<Authorization>> GetAuthorizationsByApplicationAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            return DbContext.Authorizations.Where(a => a.ApplicationId == applicationId).ToListAsync(cancellationToken);
        }

        public Task<List<Authorization>> GetAuthorizationsByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var list = DbContext.Authorizations
                .Join(DbContext.Applications, a => a.ApplicationId, app => app.Id,
                    (a, app) => new {Authorization = a, App = app});

            foreach (var x in list)
            {
                x.Authorization.Application = x.App;
            }

            return list.Select(x => x.Authorization).ToListAsync(cancellationToken);
        }
    }
}
