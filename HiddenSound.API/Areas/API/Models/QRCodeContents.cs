using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.API.Models
{
    public class QRCodeContents
    {
        public string applicationName { get; set; }

        public string authorizationCode { get; set; }
    }
}
