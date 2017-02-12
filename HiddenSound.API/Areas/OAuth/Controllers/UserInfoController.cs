using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OpenIddict.Core;

namespace HiddenSound.API.Areas.OAuth.Controllers
{
    [Route(OAuthConstants.ControllerRoute)]
    [Area("OAuth")]
    public class UserInfoController : Controller
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [Route(OAuthConstants.UserInfoRoute)]
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> Userinfo()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The user profile is no longer available."
                });
            }

            var claims = new JObject();

            // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
            claims[OpenIdConnectConstants.Claims.Subject] = await UserManager.GetUserIdAsync(user);

            if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIdConnectConstants.Scopes.Email))
            {
                claims[OpenIdConnectConstants.Claims.Email] = await UserManager.GetEmailAsync(user);
                claims[OpenIdConnectConstants.Claims.EmailVerified] = await UserManager.IsEmailConfirmedAsync(user);
            }

            if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIdConnectConstants.Scopes.Phone))
            {
                claims[OpenIdConnectConstants.Claims.PhoneNumber] = await UserManager.GetPhoneNumberAsync(user);
                claims[OpenIdConnectConstants.Claims.PhoneNumberVerified] = await UserManager.IsPhoneNumberConfirmedAsync(user);
            }

            if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIddictConstants.Scopes.Roles))
            {
                claims["roles"] = JArray.FromObject(await UserManager.GetRolesAsync(user));
            }

            // Note: the complete list of standard claims supported by the OpenID Connect specification
            // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

            return Json(claims);
        }
    }
}
