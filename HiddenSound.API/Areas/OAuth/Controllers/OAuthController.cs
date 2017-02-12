using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Core;
using OpenIddict.Models;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace HiddenSound.API.Areas.OAuth.Controllers
{
    [Route(OAuthConstants.ControllerRoute)]
    [Area("OAuth")]
    public class OAuthController : Controller
    {
        public OpenIddictApplicationManager<OpenIddictApplication<int>> ApplicationManager { get; set; }

        public SignInManager<HiddenSoundUser> SignInManager { get; set; }

        public UserManager<HiddenSoundUser> UserManager { get; set; }

        [Authorize]
        [HttpGet]
        [Route("Authorize")]
        public async Task<IActionResult> Authorize([FromQuery] OpenIdConnectRequest request)
        {
            return Redirect("");
        }

        [HttpPost]
        [Route(OAuthConstants.TokenRoute)]
        [ProducesResponseType(typeof(SignInResult), 200)]
        [ProducesResponseType(typeof(OpenIdConnectResponse), 400)]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Token([FromBody] OpenIdConnectRequest request)
        {
            if (request.IsPasswordGrantType())
            {
                var user = await UserManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }

                // Ensure the user is allowed to sign in.
                if (!await SignInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user is not allowed to sign in."
                    });
                }

                // Reject the token request if two-factor authentication has been enabled by the user.
                if (UserManager.SupportsUserTwoFactor && await UserManager.GetTwoFactorEnabledAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user is not allowed to sign in."
                    });
                }

                // Ensure the user is not already locked out.
                if (UserManager.SupportsUserLockout && await UserManager.IsLockedOutAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }

                // Ensure the password is valid.
                if (!await UserManager.CheckPasswordAsync(user, request.Password))
                {
                    if (UserManager.SupportsUserLockout)
                    {
                        await UserManager.AccessFailedAsync(user);
                    }

                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }

                if (UserManager.SupportsUserLockout)
                {
                    await UserManager.ResetAccessFailedCountAsync(user);
                }

                var ticket = await CreateTicketAsync(request, user);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, HiddenSoundUser user,
            AuthenticationProperties properties = null)
        {
            var principal = await SignInManager.CreateUserPrincipalAsync(user);
                
            // add token to destinations so the user can be serialized into the token
            foreach (var claim in principal.Claims)
            {                
                claim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken, 
                    OpenIdConnectConstants.Destinations.IdentityToken);
            }

            var ticket = new AuthenticationTicket(principal, properties, OpenIdConnectServerDefaults.AuthenticationScheme);

            if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
            {
                ticket.SetScopes(new[] {
                    OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.Profile,
                    // OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles
                }.Intersect(request.GetScopes()));
            }

            return ticket;
        }
    }
}
