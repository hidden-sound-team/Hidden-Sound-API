using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Exceptions;
using HiddenSound.API.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;

namespace HiddenSound.API.Messaging
{
    public class EmailService : IEmailService
    {
        public IEmailWebFactory EmailWebFactory { get; set; }

        public void SendEmail(string emailAddress)
        {
            var message = new SendGridMessage();
            message.AddTo(emailAddress);
            message.From = new MailAddress("admin@hiddensound.net", "HiddenSound Admin");
            message.Subject = "Hello World!";
            message.Text = "Hello World Again!";

            var web = EmailWebFactory.Create();
            web.DeliverAsync(message).Wait();
        }
    }
}
