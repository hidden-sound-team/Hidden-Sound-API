using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Mobile.Models.Requests;
using HiddenSound.API.Areas.Mobile.Models.Responses;
using HiddenSound.API.Areas.Shared.Repositories;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("Mobile/[controller]")]
    public class DevicesController : Controller
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public IDeviceRepository DeviceRepository { get; set; }

        [HttpPost("[action]")]
        [Authorize("Application")]
        public IActionResult Link([FromBody] DeviceLinkRequest request)
        {
            if (ModelState.IsValid)
            {

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(CheckDeviceResponse), 200)]
        public async Task<IActionResult> Check([FromBody] CheckDeviceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await UserManager.GetUserAsync(User);

            var response = new CheckDeviceResponse()
            {
                IsLinked = DeviceRepository.HasDevice(user, request.IMEI)
            };

            return Ok(response);
        }
    }
}
