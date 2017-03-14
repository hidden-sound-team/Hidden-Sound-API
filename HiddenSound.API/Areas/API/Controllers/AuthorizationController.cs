using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using HiddenSound.API.Areas.API.Models;
using HiddenSound.API.Areas.API.Models.Responses;
using HiddenSound.API.Areas.API.Services;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Areas.Shared.Repositories;
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
            
            var salt = new byte[256 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var authorizationCode = Convert.ToBase64String(salt);

            var qrContents = new QRCodeContents()
            {
                authorizationCode = authorizationCode,
                applicationName = "Hidden Sound Front-end"
            };

            var json = JsonConvert.SerializeObject(qrContents);

            var qrCode = QRService.Create(json);

            var authorization = new Authorization
            {
                Code = authorizationCode,
                ExpiresOn = DateTime.UtcNow.AddSeconds(5 * 60),
                Status = AuthorizationStatus.Pending,
                UserId = user.Id,
                //VendorId = user.Id
            };

            AuthorizationRepository.CreateAuthorization(authorization);

            var response = new AuthorizationCreateResponse
            {
                AuthorizationCode = authorizationCode,
                Base64QR = qrCode
            };
            return Json(response);
        }

        [HttpPost("[action]")]
        [Authorize("Application")]
        public async Task<ActionResult> Approve(string authorizationCode)
        {
            var user = await UserManager.GetUserAsync(User);

            var authorization = AuthorizationRepository.GetAuthorization(authorizationCode);

            if (authorization == null || authorization.UserId != user.Id)
            {
                return BadRequest();
            }

            authorization.Status = AuthorizationStatus.Approved;

            AuthorizationRepository.UpdateAuthorization(authorization);

            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize("Application")]
        public async Task<ActionResult> Decline(string authorizationCode)
        {
            var user = await UserManager.GetUserAsync(User);

            var authorization = AuthorizationRepository.GetAuthorization(authorizationCode);

            if (authorization.UserId != user.Id)
            {
                return BadRequest();
            }

            authorization.Status = AuthorizationStatus.Declined;

            AuthorizationRepository.UpdateAuthorization(authorization);

            return Ok();
        }

        [HttpGet("[action]")]
        [Authorize("Api")]
        [ProducesResponseType(typeof(AuthorizationStatusResponse), 200)]
        [ProducesResponseType(typeof(AuthorizationStatusResponse), 404)]
        public ActionResult Status([FromQuery] string authorizationCode)
        {
            var authorization = AuthorizationRepository.GetAuthorization(authorizationCode);

            if (authorization == null)
            {
                return NotFound();
            }

            // we won't do any updating here. expired date is already stored
            // and we can use batch sql update job to remove expired authorizations
            if (DateTime.UtcNow >= authorization.ExpiresOn)
            {
                authorization.Status = AuthorizationStatus.Expired;
            }

            var response = new AuthorizationStatusResponse
            {
                Status = authorization.Status
            };
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Authorize("Api")]
        [ProducesResponseType(typeof(Authorization), 200)]
        [ProducesResponseType(typeof(AuthorizationStatusResponse), 404)]
        public ActionResult Info([FromQuery] string authorizationCode)
        {
            var authorization = AuthorizationRepository.GetAuthorization(authorizationCode);

            if (authorization == null)
            {
                return NotFound();
            }

            // transaction.Base64QR = QRService.Create(transaction.AuthorizationCode);

            return Json(authorization);
        }
    }
}
