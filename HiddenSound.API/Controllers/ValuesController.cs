using System.Collections.Generic;
using HiddenSound.API.Messaging;
using HiddenSound.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public IValuesRepository ValuesRepository { get; set; }

        public IEmailService EmailService { get; set; }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return ValuesRepository.GetValues();
        }

        // GET api/values/5
        [HttpGet("{email}")]
        public string Get(string email)
        {
            EmailService.SendEmail(email);

            return $"Email Sent to {email}!";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
