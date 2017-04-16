using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Application.Models.Requests;
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

            var devices = await DeviceRepository.GetDevicesAsync(user.Id, HttpContext.RequestAborted);

            var deviceListResponse = new DeviceListResponse()
            {
                Devices = devices.Select(d => new DeviceListResponse.Device
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList()
            };

            return Ok(deviceListResponse);
        }

        [HttpPut("{deviceId}")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(DeviceListResponse.Device), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put([FromRoute]string deviceId, [FromForm] DevicePutRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);

                var device = await DeviceRepository.GetDeviceByIdAsync(user.Id, new Guid(deviceId), HttpContext.RequestAborted);

                if (device == null)
                {
                    ModelState.AddModelError("Device", "The device is invalid");
                    return BadRequest(ModelState);
                }

                device.Name = request.Name;

                await DeviceRepository.UpdateDeviceAsync(device, HttpContext.RequestAborted);

                var response = new DeviceListResponse.Device
                {
                    Id = device.Id,
                    Name = device.Name
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{deviceId}")]
        [Authorize("Application")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete([FromRoute] string deviceId)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);

                var device = await DeviceRepository.GetDeviceByIdAsync(user.Id, new Guid(deviceId), HttpContext.RequestAborted);

                if (device == null)
                {
                    ModelState.AddModelError("Device", "The device is invalid");
                    return BadRequest(ModelState);
                }

                await DeviceRepository.RemoveDeviceAsync(device, HttpContext.RequestAborted);

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
