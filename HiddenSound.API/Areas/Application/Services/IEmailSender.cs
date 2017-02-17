using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string emailAddress, string subject, string text);
    }
}
