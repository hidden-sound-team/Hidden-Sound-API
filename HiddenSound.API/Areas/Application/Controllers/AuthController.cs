using HiddenSound.API.Areas.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.Application.Controllers
{
    [Area("Application")]
    [Route("Application/[controller]")]
    public class AuthController : Controller
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        [HttpGet]
        public JsonResult Index()
        {
            return Json("index");
        }

        [HttpPost]
        [Route("[action]")]
        public JsonResult Login(ApplicationUser user)
        {
            return Json(user);
        }
    }
}
