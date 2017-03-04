using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptions;
using HiddenSound.API.Areas.Application.Models;
using HiddenSound.API.Areas.Application.Models.Responses;
using HiddenSound.API.Areas.Application.Services;
using HiddenSound.API.Configuration;
using HiddenSound.API.Extensions;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HiddenSound.API.Areas.Application.Controllers
{
    [Area("Application")]
    [Route("Application/[controller]")]
    public class UserController : ApplicationController
    {
        public SignInManager<HiddenSoundUser> SignInManager { get; set; }

        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public IEmailSender EmailSender { get; set; }

        public IOptions<AppSettingsConfig> AppSettings { get; set; }

        [HttpGet("[action]")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(UserInfoResponse), 200)]
        public async Task<IActionResult> Info()
        {
            var user = await UserManager.GetUserAsync(User);
            var response = new UserInfoResponse
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = new HiddenSoundUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    result.AddErrors(ModelState);
                    BadRequest(ModelState);
                }

                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "User", new {userId = user.Id, code = code}, Request.Scheme);

                try
                {
                    await EmailSender.SendEmailAsync(model.Email, "Confirm your account", $"<a href=\"{callbackUrl}\">{callbackUrl}</a>");
                }
                catch (InvalidApiRequestException)
                {
                    await UserManager.DeleteAsync(user);
                    throw;
                }

                return Ok(callbackUrl);
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    ModelState.AddModelError("UserId", "Invalid user id");
                    return BadRequest(ModelState);
                }

                var result = await UserManager.ConfirmEmailAsync(user, code);

                if (!result.Succeeded)
                {
                    result.AddErrors(ModelState);
                    return BadRequest(ModelState);

                }

                return Redirect($"{AppSettings.Value.WebUri}/login");
            }

            return BadRequest(ModelState);
        }
    }
}