using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace HiddenSound.API.Areas.API.Controllers
{
    [Area("API")]
    [Route("API/[controller]")]
    public class QRController : Controller
    {
        [HttpGet]
        [Route("[action]")]
        public JsonResult Generate([FromQuery] string text)
        {
            return new JsonResult("test");
        }
    }
}
