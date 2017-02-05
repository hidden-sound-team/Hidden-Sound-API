using System;
using System.Linq;
using HiddenSound.API.Areas.Mobile.Models.Requests;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("Mobile/[controller]")]
    [EnableCors("Application")]
    public class TransactionController : Controller
    {
        [HttpPost]
        [Route("[action]")]
        public ActionResult Authorize([FromBody]TransactionAuthorizeRequest request)
        {
            return Json(request);
        }
    }
}
