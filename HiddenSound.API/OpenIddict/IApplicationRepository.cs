using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HiddenSound.API.OpenIddict
{
    public interface IApplicationRepository
    {
        Task<List<HSOpenIddictApplication>> GetApplicationsAsync(Guid userId, CancellationToken cancellationToken);

        Task<HSOpenIddictApplication> GetApplicationByClientIdAsync(Guid userId, string clientId, CancellationToken cancellationToken);
    }
}
