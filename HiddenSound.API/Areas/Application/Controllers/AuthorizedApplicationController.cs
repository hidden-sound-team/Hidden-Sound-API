using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Application.Models.Responses;
using HiddenSound.API.Areas.Shared.Repositories;
using HiddenSound.API.Identity;
using HiddenSound.API.OpenIddict;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;
using HiddenSound.API.Areas.Application.Models.Requests;

namespace HiddenSound.API.Areas.Application.Controllers
{
    [Area("Application")]
    [Route("Application/[controller]")]
    public class AuthorizedApplicationController : Controller
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public IAuthorizedApplicationRepository AuthorizedApplicationRepository { get; set; }

        public OpenIddictApplicationManager<HSOpenIddictApplication> ApplicationManager { get; set; }

        [HttpGet("[action]")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(AuthorizedApplicationListResponse), 200)]
        public async Task<IActionResult> List()
        {
            var user = await UserManager.GetUserAsync(User);

            var authorizedApplications = await AuthorizedApplicationRepository.GetAuthorizedApplicationsByUserIdAsync(user.Id, HttpContext.RequestAborted);

            var response = new AuthorizedApplicationListResponse
            {
                AuthorizedApplications = authorizedApplications.Select(a => new AuthorizedApplication
                {
                    Id = a.Application.Id,
                    Name = a.Application.DisplayName
                }).ToList()
            };

            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize("Application")]
        public async Task<IActionResult> Add([FromForm] AuthorizedApplicationAddRequest request)
        {
            var user = await UserManager.GetUserAsync(User);

            var application = await ApplicationManager.FindByClientIdAsync(request.ClientId, HttpContext.RequestAborted);

            if (application == null)
            {
                ModelState.AddModelError("Application", "The application is invalid");
                return BadRequest(ModelState);
            }

            if (await AuthorizedApplicationRepository.GetAuthorizedApplicationAsync(user.Id, application.Id,
                    HttpContext.RequestAborted) != null)
            {
                ModelState.AddModelError("Application", "The application is already authorized.");
                return BadRequest(ModelState);
            }

            var authorizedApplication = new Shared.Models.AuthorizedApplication()
            {
                UserId = user.Id,
                ApplicationId = application.Id
            };

            await AuthorizedApplicationRepository.CreateAuthorizedApplicationAsync(authorizedApplication,
                HttpContext.RequestAborted);

            return Ok();
        }

        [HttpDelete("{authorizedApplicationId}")]
        [Authorize("Application")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete([FromRoute] string authorizedApplicationId)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);

                var application = await AuthorizedApplicationRepository.GetAuthorizedApplicationAsync(user.Id, new Guid(authorizedApplicationId), HttpContext.RequestAborted);

                if (application == null)
                {
                    ModelState.AddModelError("Application", "The authorized application is invalid");
                    return BadRequest(ModelState);
                }

                await AuthorizedApplicationRepository.RemoveAuthorizedApplicationAsync(application, HttpContext.RequestAborted);

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
