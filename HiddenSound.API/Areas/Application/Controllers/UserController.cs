using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

                if (result.Succeeded)
                {
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = $"{AppSettings.Value.WebUrl}/ConfirmEmail?userId={user.Id}&code={code}";
                    await EmailSender.SendEmailAsync(model.Email, "Confirm your account",
                        "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");

                    return Ok();
                }

                result.AddErrors(ModelState);
            }

            return BadRequest(ModelState);
        }

        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            if (ModelState.IsValid)
            {
                
            }

            return BadRequest(ModelState);
        }
    }
}