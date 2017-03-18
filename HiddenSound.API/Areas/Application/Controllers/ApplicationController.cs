using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Application.Models;
using HiddenSound.API.Areas.Application.Models.Requests;
using HiddenSound.API.Areas.Application.Models.Responses;
using HiddenSound.API.Areas.Shared.Repositories;
using HiddenSound.API.Helpers;
using HiddenSound.API.Identity;
using HiddenSound.API.OpenIddict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Core;

namespace HiddenSound.API.Areas.Application.Controllers
{
    [Area("Application")]
    [Route("Application/[controller]")]
    public class ApplicationController : Controller
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public OpenIddictApplicationManager<HSOpenIddictApplication> ApplicationManager { get; set; }

        public IApplicationRepository ApplicationRepository { get; set; }

        public IAuthorizationRepository AuthorizationRepository { get; set; }

        [HttpGet("[action]")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(ApplicationListResponse), 200)]
        public async Task<IActionResult> List()
        {
            var user = await UserManager.GetUserAsync(User);

            var applications = await ApplicationRepository.GetApplicationsAsync(user.Id, HttpContext.RequestAborted);
            
            var response = new ApplicationListResponse
            {
                Applications = applications.Select(app => new ApplicationModel
                {
                    Name = app.DisplayName,
                    ClientId = app.ClientId,
                    RedirectUri = app.RedirectUri
                }).ToList()
            };

            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(ApplicationModel), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromForm] ApplicationCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);

                var application = new HSOpenIddictApplication
                {
                    DisplayName = request.Name,
                    RedirectUri = request.RedirectUri,
                    ClientId = RandomPassword.Generate(25, true, true, true, false),
                    UserId = user.Id
                };

                var clientSecret = RandomPassword.Generate(50, true, true, true, false);

                await ApplicationManager.CreateAsync(application, clientSecret, HttpContext.RequestAborted);

                var response = new ApplicationModel
                {
                    ClientId = application.ClientId,
                    ClientSecret = clientSecret,
                    RedirectUri = application.RedirectUri,
                    Name = application.DisplayName
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{clientId}")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(ApplicationModel), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put([FromRoute] string clientId, [FromForm] ApplicationInfoPutRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);

                var application = await ApplicationRepository.GetApplicationByClientIdAsync(user.Id, clientId, HttpContext.RequestAborted);

                if (application == null)
                {
                    ModelState.AddModelError("Application", "The application is invalid");
                    return BadRequest(ModelState);
                }

                application.DisplayName = request.Name;
                application.RedirectUri = request.RedirectUri;

                await ApplicationManager.UpdateAsync(application, HttpContext.RequestAborted);

                var response = new ApplicationModel
                {
                    ClientId = application.ClientId,
                    RedirectUri = application.RedirectUri,
                    Name = application.DisplayName
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{clientId}")]
        [Authorize("Application")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete([FromRoute] string clientId)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);

                var application = await ApplicationRepository.GetApplicationByClientIdAsync(user.Id, clientId, HttpContext.RequestAborted);

                if (application == null)
                {
                    ModelState.AddModelError("Application", "The application is invalid");
                    return BadRequest(ModelState);
                }

                await AuthorizationRepository.RemoveAuthorizationsByApplicationAsync(application.Id, HttpContext.RequestAborted);
                await ApplicationManager.DeleteAsync(application, HttpContext.RequestAborted);

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
