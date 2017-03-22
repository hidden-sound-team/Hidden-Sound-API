using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoHelper;
using HiddenSound.API.Areas.Mobile.Models.Requests;
using HiddenSound.API.Areas.Mobile.Models.Responses;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Areas.Shared.Repositories;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("Mobile/[controller]")]
    public class DeviceController : Controller
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public IDeviceRepository DeviceRepository { get; set; }

        [HttpPost("[action]")]
        [Authorize("Application")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Link([FromForm] DeviceLinkRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);

                if (user == null)
                {
                    ModelState.AddModelError("User", "The user is invalid");
                    return BadRequest(ModelState);
                }

                if (await DeviceRepository.GetDeviceAsync(request.IMEI, HttpContext.RequestAborted) != null)
                {
                    ModelState.AddModelError("Device", "The device is already linked");
                    return BadRequest(ModelState);
                }

                var device = new Device
                {
                    IMEI = Crypto.HashPassword(request.IMEI),
                    Name = request.Name,
                    UserId = user.Id
                };

                await DeviceRepository.AddDeviceAsync(device, HttpContext.RequestAborted);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(CheckDeviceResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Check([FromForm] DeviceCheckRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);
                
                var response = new CheckDeviceResponse
                {
                    IsUserDevice = await DeviceRepository.GetDeviceAsync(user, request.IMEI, HttpContext.RequestAborted) != null,
                    IsDeviceLinked = await DeviceRepository.GetDeviceAsync(request.IMEI, HttpContext.RequestAborted) != null,
                    UserHasDevice = (await DeviceRepository.GetDevicesAsync(user, HttpContext.RequestAborted)).Count > 0
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }
    }
}
