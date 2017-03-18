using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Application.Models.Responses;
using HiddenSound.API.Areas.Shared.Repositories;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.Application.Controllers
{
    [Area("Application")]
    [Route("Application/[controller]")]
    public class DeviceController : Controller 
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public IDeviceRepository DeviceRepository { get; set; }

        [HttpGet("[action]")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(DeviceListResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> List()
        {
            var user = await UserManager.GetUserAsync(User);

            if (user == null)
            {
                ModelState.AddModelError("User", "The user is invalid");
                return BadRequest(ModelState);
            }

            var devices = await DeviceRepository.GetDevicesAsync(user, HttpContext.RequestAborted);

            var deviceListResponse = new DeviceListResponse()
            {
                Devices = devices.Select(d => new DeviceListResponse.Device()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList()
            };

            return Ok(deviceListResponse);
        }
    }
}
