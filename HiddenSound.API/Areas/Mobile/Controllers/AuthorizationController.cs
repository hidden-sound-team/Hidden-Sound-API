using System;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Mobile.Models.Requests;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("Mobile/[controller]")]
    public class AuthorizationController : Controller
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        [HttpPost("[action]")]
        [Authorize("Application")]

        public async Task<IActionResult> Approve([FromBody]AuthorizationAuthorizeRequest request)
        {
            var user = await UserManager.GetUserAsync(User);



            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize("Application")]
        public async Task<IActionResult> Decline([FromBody]AuthorizationAuthorizeRequest request)
        {
            return Ok();
        }
    }
}
