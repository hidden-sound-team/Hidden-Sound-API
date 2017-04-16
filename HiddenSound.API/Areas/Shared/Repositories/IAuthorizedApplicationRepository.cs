using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public interface IAuthorizedApplicationRepository
    {
        Task<List<AuthorizedApplication>> GetAuthorizedApplicationsByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        Task<AuthorizedApplication> GetAuthorizedApplicationAsync(Guid userId, Guid applicationId, CancellationToken cancellationToken);

        Task CreateAuthorizedApplicationAsync(AuthorizedApplication authorizedApplication, CancellationToken cancellationToken);

        Task RemoveAuthorizedApplicationAsync(AuthorizedApplication authorizedApplication, CancellationToken cancellationToken);
    }
}
