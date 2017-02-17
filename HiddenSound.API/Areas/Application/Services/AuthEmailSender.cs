using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using HiddenSound.API.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;

namespace HiddenSound.API.Areas.Application.Services
{
    public class AuthEmailSender : IEmailSender
    {
        public IOptions<SendGridConfig> SendGridConfig { get; set; }

        public Task SendEmailAsync(string emailAddress, string subject, string text)
        {
            var message = new SendGridMessage();
            message.AddTo(emailAddress);
            message.From = new MailAddress("admin@hiddensound.net", "HiddenSound");
            message.Subject = subject;
            message.Text = text;
            message.Html = text;

            var web = new Web(SendGridConfig.Value.APIKey);
            return web.DeliverAsync(message);
        }
    }
}
