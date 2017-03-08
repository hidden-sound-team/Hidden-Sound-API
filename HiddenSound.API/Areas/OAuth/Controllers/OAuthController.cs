using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using HiddenSound.API.Configuration;
using HiddenSound.API.Extensions;
using HiddenSound.API.Helpers;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using OpenIddict.Core;
using OpenIddict.Models;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;
using HiddenSound.API.OpenIddict;
using System.Net;

namespace HiddenSound.API.Areas.OAuth.Controllers
{
    [Route(OAuthConstants.ControllerRoute)]
    [Area("OAuth")]
    public class OAuthController : Controller
    {
        public OpenIddictApplicationManager<HSOpenIddictApplication> ApplicationManager { get; set; }

        public SignInManager<HiddenSoundUser> SignInManager { get; set; }

        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public IOptions<AppSettingsConfig> AppSettings { get; set; }

        [HttpGet(OAuthConstants.AuthorizeRoute)]
        public async Task<IActionResult> Authorize([FromQuery] OpenIdConnectRequest request)
        {
            var application = await ApplicationManager.FindByClientIdAsync(request.ClientId, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidClient,
                    ErrorDescription = "Details concerning the calling client application cannot be found in the database"
                });
            }

            var redirectUri = QueryHelpers.AddQueryString($"{AppSettings.Value.WebUri}/authorize",
                new Dictionary<string, string>()
                {
                    { "application", WebUtility.UrlEncode(application.DisplayName) },
                    { "request_id", WebUtility.UrlEncode(request.RequestId) },
                    { "scope", WebUtility.UrlEncode(request.Scope) ?? string.Empty }
                });

            return Redirect(redirectUri);
        }

        [Authorize]
        [HttpPost(OAuthConstants.AuthorizeRoute)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AuthorizePost([FromBody] OpenIdConnectRequest request)
        {
            if (!string.IsNullOrEmpty(HttpContext.Request.Form["submit.Deny"]))
            {
                return Forbid(OpenIdConnectServerDefaults.AuthenticationScheme);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.ServerError,
                    ErrorDescription = "An internal error has occurred"
                });
            }

            var principal = await SignInManager.CreateUserPrincipalAsync(user);
            principal.AddClaim("api", "true", ClaimValueTypes.Boolean);
            var ticket = CreateTicket(request, principal);
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }

        [HttpPost(OAuthConstants.TokenRoute)]
        [ProducesResponseType(typeof(SignInResult), 200)]
        [ProducesResponseType(typeof(OpenIdConnectResponse), 400)]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Token([FromBody] OpenIdConnectRequest request)
        {
            if (request.IsAuthorizationCodeGrantType())
            {
                var info = await HttpContext.Authentication.GetAuthenticateInfoAsync(OpenIdConnectServerDefaults.AuthenticationScheme);

                var user = await UserManager.GetUserAsync(info.Principal);
                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The authorization code is no longer valid."
                    });
                }

                if (!await SignInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The user is no longer allowed to sign in."
                    });
                }

                var principal = await SignInManager.CreateUserPrincipalAsync(user);
                var ticket = CreateTicket(request, principal, info.Properties);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

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
                
                if (!await SignInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "You must have a confirmed email to log in."
                    });
                }
                
                if (UserManager.SupportsUserTwoFactor && await UserManager.GetTwoFactorEnabledAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user is not allowed to sign in."
                    });
                }
                
                if (UserManager.SupportsUserLockout && await UserManager.IsLockedOutAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }
                
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

                var principal = await SignInManager.CreateUserPrincipalAsync(user);
                principal.AddClaim("application", "true", ClaimValueTypes.Boolean);
                principal.AddClaim("api", "true", ClaimValueTypes.Boolean);
                var ticket = CreateTicket(request, principal);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
        }

        private AuthenticationTicket CreateTicket(OpenIdConnectRequest request, ClaimsPrincipal principal, AuthenticationProperties properties = null)
        {
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
                    OpenIdConnectConstants.Scopes.OpenId
                    // OpenIdConnectConstants.Scopes.Email,
                    // OpenIdConnectConstants.Scopes.Profile,
                    // OpenIdConnectConstants.Scopes.OfflineAccess,
                    // OpenIddictConstants.Scopes.Roles
                }.Intersect(request.GetScopes()));
            }

            return ticket;
        }
    }
}
