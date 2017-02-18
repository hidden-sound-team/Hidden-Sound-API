using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptions;
using HiddenSound.API.Areas.Application.Models;
using HiddenSound.API.Areas.Application.Services;
using HiddenSound.API.Configuration;
using HiddenSound.API.Extensions;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = new HiddenSoundUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    result.AddErrors(ModelState);
                    BadRequest(ModelState);
                }

                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = $"{AppSettings.Value.WebUrl}/ConfirmEmail?userId={user.Id}&code={code}";
                try
                {
                    await EmailSender.SendEmailAsync(model.Email, "Confirm your account", $"Please confirm your account by clicking this link: <a href=\"{callbackUrl}\">{callbackUrl}</a>");
                }
                catch (InvalidApiRequestException)
                {
                    await UserManager.DeleteAsync(user);
                    throw;
                }

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(request.UserID.ToString());

                if (user == null)
                {
                    ModelState.AddModelError("UserID", "Invalid user id");
                    return BadRequest(ModelState);
                }

                var result = await UserManager.ConfirmEmailAsync(user, request.Code);

                if (!result.Succeeded)
                {
                    result.AddErrors(ModelState);
                    return BadRequest(ModelState);

                }

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}