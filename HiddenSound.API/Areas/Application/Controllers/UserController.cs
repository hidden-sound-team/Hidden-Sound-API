using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptions;
using HiddenSound.API.Areas.Application.Models;
using HiddenSound.API.Areas.Application.Models.Requests;
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
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public IEmailSender EmailSender { get; set; }

        public IOptions<AppSettingsConfig> AppSettings { get; set; }

        [HttpGet("Info")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(UserInfoResponse), 200)]
        public async Task<IActionResult> InfoGet()
        {
            var user = await UserManager.GetUserAsync(User);
            var response = new UserInfoResponse
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName
            };

            return Ok(response);
        }

        [HttpPut("Info")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(UserInfoResponse), 200)]
        public async Task<IActionResult> InfoPut([FromForm] UserInfoPutRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;

                var result = await UserManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    result.AddErrors(ModelState);
                    BadRequest(ModelState);
                }

                var response = new UserInfoResponse
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName
                };
                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
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

        [HttpGet("[action]")]
        [AllowAnonymous]
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

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("Invalid email");
            }

            if (!await UserManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest("User has not confirmed their email");
            }

            var token = await UserManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = $"{AppSettings.Value.WebUri}/resetpassword?token={token}";
            await EmailSender.SendEmailAsync(email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>{callbackUrl}</a>");

            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize("Application")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);

                var result = await UserManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

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