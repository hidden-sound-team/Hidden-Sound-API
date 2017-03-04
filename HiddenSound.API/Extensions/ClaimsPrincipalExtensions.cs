using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;

namespace HiddenSound.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        private static readonly Regex ScopeRegex = new Regex("\"([a-zA-Z0-9]+)\"");

        public static bool HasScope(this ClaimsPrincipal claimsPrincipal, string scope)
        {
            return claimsPrincipal.HasClaim(c => c.Type == OpenIdConnectConstants.Claims.Scope
                                                 && ScopeRegex.Matches(c.Value)
                                                     .Cast<Match>()
                                                     .Any(m => m.Groups[1].Value.Equals(scope, StringComparison.OrdinalIgnoreCase)));
        }

        public static void AddClaim(this ClaimsPrincipal claimsPrincipal, string type, string value, string valueType)
        {
            var identity = (ClaimsIdentity)claimsPrincipal.Identity;
            var officeClaim = new Claim(type, value, valueType);
            identity.AddClaim(officeClaim);
        }
    }
}
