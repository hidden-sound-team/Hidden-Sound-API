using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Application.Models.Responses;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.Application.Controllers
{
    [Area("Application")]
    [Route("Application/[controller]")]
    public class DevicesController : Controller 
    {
        public UserManager<HiddenSoundUser> UserManager { get; set; }

        public HiddenSoundDbContext HiddenSoundDbContext { get; set; }

        [HttpGet("[action]")]
        [Authorize("Application")]
        [ProducesResponseType(typeof(DeviceListResponse), 200)]
        public async Task<IActionResult> List()
        {
            var user = await UserManager.GetUserAsync(User);

            var deviceListResponse = new DeviceListResponse()
            {
                Devices = HiddenSoundDbContext.Devices.Where(d => d.UserId == user.Id).Select(d => new Device()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList()
            };

            return Ok(deviceListResponse);
        }
    }
}
