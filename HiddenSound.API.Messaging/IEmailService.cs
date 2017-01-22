using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Messaging
{
    public interface IEmailService
    {
        void SendEmail(string emailAddress);
    }
}
