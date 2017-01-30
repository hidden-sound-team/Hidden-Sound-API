using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Exceptions;
using HiddenSound.API.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;

namespace HiddenSound.API.Messaging
{
    public class EmailService : IEmailService
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public IEmailWebFactory EmailWebFactory { get; set; }

        public void SendEmail(string emailAddress, string subject, string text)
        {
            var message = new SendGridMessage();
            message.AddTo(emailAddress);
            message.From = new MailAddress("admin@hiddensound.net", "HiddenSound");
            message.Subject = subject;
            message.Text = text;

            var web = EmailWebFactory.Create();
            web.DeliverAsync(message).Wait();
        }
    }
}
