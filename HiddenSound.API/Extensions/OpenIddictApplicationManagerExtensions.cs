using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using HiddenSound.API.OpenIddict;
using OpenIddict.Core;

namespace HiddenSound.API.Extensions
{
    public static class OpenIddictApplicationManagerExtensions
    {
        public static async Task<HSOpenIddictApplication> GetApplicationAync(this OpenIddictApplicationManager<HSOpenIddictApplication> manager, ClaimsPrincipal principal, CancellationToken cancellationToken)
        {
            var applicationId = principal.GetClaim(CustomClaimTypes.ApplicationId);
            
            if (applicationId == null)
            {
                return null;
            }

            return await manager.FindByIdAsync(applicationId, cancellationToken);
        }
    }
}
