using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;

namespace HiddenSound.API.Messaging
{
    public interface IEmailWebFactory
    {
        Web Create();
    }
}
