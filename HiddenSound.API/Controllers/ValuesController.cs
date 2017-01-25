using System.Collections.Generic;
using HiddenSound.API.Areas.API.Models;
using HiddenSound.API.Messaging;
using HiddenSound.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Controllers
{
    [Route("Api/[controller]")]
    public class ValuesController : Controller
    {
        public IValuesRepository ValuesRepository { get; set; }

        public IEmailService EmailService { get; set; }

        public IAPIKeyRepository ApiKeyRepository { get; set; }

        [HttpPost]
        [Route("[action]")]
        public JsonResult SendEmail(string email)
        {
            EmailService.SendEmail(email);

            return Json("Email Sent!");
        }

        [HttpGet]
        [Route("[action]/{userId}")]
        public JsonResult MyTest(string userId, string someOtherId)
        {
            return Json($"{userId} - {someOtherId}");
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<APIKey> GetApiKeys(string userId)
        {
            return ApiKeyRepository.GetAPIKeys();
        }
    }
}
