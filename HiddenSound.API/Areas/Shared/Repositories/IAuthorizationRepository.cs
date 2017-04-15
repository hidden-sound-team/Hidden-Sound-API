using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public interface IAuthorizationRepository
    {
        List<Authorization> GetAllAuthorizations();

        Authorization GetAuthorization(string code);

        Task<Authorization> GetAuthorizationAsync(Guid application, string code, CancellationToken cancellationToken);

        void UpdateAuthorization(Authorization authorization);

        void CreateAuthorization(Authorization authorization);

        Task RemoveAuthorizationsByApplicationAsync(Guid applicationId, CancellationToken cancellationToken);

        Task<List<Authorization>> GetAuthorizationsByApplicationAsync(Guid applicationId, CancellationToken cancellationToken);

        Task<List<Authorization>> GetAuthorizationsByUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}
