using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HiddenSound.API.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;

namespace HiddenSound.API.Messaging
{
    public class EmailWebFactory : IEmailWebFactory
    {
        public IOptions<SendGridConfig> SendGridConfig { get; set; }

        public Web Create()
        {
            return new Web(SendGridConfig.Value.APIKey);
        }
    }
}
