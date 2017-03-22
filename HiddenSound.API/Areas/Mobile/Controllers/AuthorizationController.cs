using System;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Mobile.Models.Requests;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Areas.Shared.Repositories;
using HiddenSound.API.Hubs;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HiddenSound.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("Mobile/[controller]")]
    public class AuthorizationController : Controller
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public IAuthorizationRepository AuthorizationRepository { get; set; }

        public IDeviceRepository DeviceRepository { get; set; }

        public IHubContext<AuthorizationHub, IClient> AuthorizationHub { get; set; }

        [HttpPost("[action]")]
        [Microsoft.AspNetCore.Authorization.Authorize("Application")]

        public async Task<IActionResult> Approve([FromForm] AuthorizationAuthorizeRequest request)
        {
            if (ModelState.IsValid)
            {
                return await Authorize(request, true);
            }
            
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        [Microsoft.AspNetCore.Authorization.Authorize("Application")]
        public async Task<IActionResult> Decline([FromForm] AuthorizationAuthorizeRequest request)
        {
            if (ModelState.IsValid)
            {
                return await Authorize(request, false);
            }

            return BadRequest(ModelState);
        }

        private async Task<IActionResult> Authorize(AuthorizationAuthorizeRequest request, bool approve)
        {
            var user = await UserManager.GetUserAsync(User);
            var authorization = AuthorizationRepository.GetAuthorization(request.AuthorizationCode);

            if (authorization == null || authorization.UserId != user.Id)
            {
                ModelState.AddModelError("Authorization", "The authorization is invalid");
                return BadRequest(ModelState);
            }

            if (authorization.ExpiresOn < DateTime.UtcNow)
            {
                ModelState.AddModelError("Authorization", "The authorization has expired");
                return BadRequest(ModelState);
            }

            var device = await DeviceRepository.GetDeviceAsync(user, request.IMEI, HttpContext.RequestAborted);

            if (device == null)
            {
                ModelState.AddModelError("Device", "The device is invalid");
                return BadRequest(ModelState);
            }

            authorization.Status = approve ? AuthorizationStatus.Approved : AuthorizationStatus.Declined;

            AuthorizationRepository.UpdateAuthorization(authorization);

            AuthorizationHub.Clients.User(user.Id.ToString()).UpdateAuthorization(authorization.Status);

            return Ok();
        }
    }
}
