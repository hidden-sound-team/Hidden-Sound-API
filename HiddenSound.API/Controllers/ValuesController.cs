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

        [HttpGet]
        public IEnumerable<APIKey> Index()
        {
            return ApiKeyRepository.GetAPIKeys();
        }
    }
}
