using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Mobile.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("Mobile/[controller]")]
    public class DevicesController : Controller
    {
        [HttpPost("[action]")]
        [Authorize]
        [ProducesResponseType(400)]
        public IActionResult Link([FromBody] DeviceLinkRequest request)
        {
            if (ModelState.IsValid)
            {

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
