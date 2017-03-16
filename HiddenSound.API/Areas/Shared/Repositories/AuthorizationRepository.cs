using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;
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
    }
}
