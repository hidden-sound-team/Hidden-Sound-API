using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using HiddenSound.API.Areas.API.Models;
using HiddenSound.API.Areas.API.Models.Responses;
using HiddenSound.API.Areas.API.Services;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Areas.Shared.Repositories;
using HiddenSound.API.Extensions;
using HiddenSound.API.Identity;
using HiddenSound.API.OpenIddict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenIddict.Core;

namespace HiddenSound.API.Areas.API.Controllers
{
    [Area("API")]
    [Route("Api/[controller]")]
    public class AuthorizationController : Controller
    {
        public IQRService QRService { get; set; }

        public IAuthorizationRepository AuthorizationRepository { get; set; }

        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public OpenIddictApplicationManager<HSOpenIddictApplication> ApplicationManager { get; set; }

        [HttpPost("[action]")]
        [Authorize("Api")]
        [ProducesResponseType(typeof(AuthorizationCreateResponse), 200)]
        public async Task<ActionResult> Create()
        {
            var user = await UserManager.GetUserAsync(User);

            if (user == null)
            {
                ModelState.AddModelError("User", "The user is invalid");
                return BadRequest(ModelState);
            }

            var application = await ApplicationManager.GetApplicationAync(User, HttpContext.RequestAborted);

            if (application == null)
            {
                ModelState.AddModelError("Application", "The application is invalid");
                return BadRequest(ModelState);
            }
            
            var salt = new byte[256 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var authorizationCode = Convert.ToBase64String(salt);

            var qrContents = new QRCodeContents()
            {
                authorizationCode = authorizationCode,
                applicationName = "Board Shop"
            };

            var json = JsonConvert.SerializeObject(qrContents);

            var qrCode = QRService.Create(json);

            var authorization = new Authorization
            {
                Code = authorizationCode,
                ExpiresOn = DateTime.UtcNow.AddSeconds(5 * 60),
                CreatedOn = DateTime.UtcNow,
                Status = AuthorizationStatus.Pending,
                UserId = user.Id,
                ApplicationId = application.Id
            };

            AuthorizationRepository.CreateAuthorization(authorization);

            var response = new AuthorizationCreateResponse
            {
                AuthorizationCode = authorizationCode,
                Base64QR = qrCode,
                ExpiresOn = authorization.ExpiresOn,
                CreatedOn = authorization.CreatedOn,
                Status = authorization.Status
            };

            return Ok(response);
        }

        [HttpGet("[action]")]
        [Authorize("Api")]
        [ProducesResponseType(typeof(AuthorizationInfoResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Info([FromQuery] string authorizationCode, [FromQuery] bool includeQrImage = false)
        {
            var application = await ApplicationManager.GetApplicationAync(User, HttpContext.RequestAborted);

            if (application == null)
            {
                ModelState.AddModelError("Application", "The application is invalid");
                return BadRequest(ModelState);
            }

            var authorization = await AuthorizationRepository.GetAuthorizationAsync(application.Id, authorizationCode, HttpContext.RequestAborted);

            if (authorization == null)
            {
                return NotFound();
            }

            if (DateTime.UtcNow >= authorization.ExpiresOn)
            {
                authorization.Status = AuthorizationStatus.Expired;
            }

            var response = new AuthorizationInfoResponse()
            {
                Base64QR = includeQrImage ? QRService.Create(authorization.Code) : null,
                Status =  authorization.Status
            };

            return Ok(response);
        }
    }
}
