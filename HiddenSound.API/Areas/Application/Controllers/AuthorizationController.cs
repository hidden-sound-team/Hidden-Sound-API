using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Application.Models.Responses;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Areas.Shared.Repositories;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.Application.Controllers
{
    [Area("Application")]
    [Route("Application/[controller]")]
    public class AuthorizationController : Controller
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public IAuthorizationRepository AuthorizationRepository { get; set; }

        [HttpGet("[action]")]
        [Authorize("Application")]
        public async Task<IActionResult> History()
        {
            var user = await UserManager.GetUserAsync(User);

            var authorizations = await AuthorizationRepository.GetAuthorizationsByUserAsync(user.Id, HttpContext.RequestAborted);

            var response = new AuthorizationHistoryResponse()
            {
                Authorizations = authorizations.OrderByDescending(a => a.CreatedOn).Select(a => new AuthorizationHistoryResponse.Authorization()
                {
                    AppName = a.Application.DisplayName,
                    CreatedOn = a.CreatedOn,
                    Status = DateTime.UtcNow > a.ExpiresOn ? AuthorizationStatus.Expired.ToString() : a.Status.ToString()
                }).ToList()
            };

            return Ok(response);
        }
    }
}
