using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.API.Controllers
{
    [Area("API")]
    [Route("Api/[controller]")]
    public class TestController : Controller
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        [HttpGet]
        [Route("[action]")]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Message()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            return Content($"If this is being shown, then {user.UserName} has sucessfully accessed an api route that needs oauth authorization");
        }
    }
}
