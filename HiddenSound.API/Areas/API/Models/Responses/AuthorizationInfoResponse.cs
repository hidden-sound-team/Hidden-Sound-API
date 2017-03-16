using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;

namespace HiddenSound.API.Areas.API.Models.Responses
{
    public class AuthorizationInfoResponse
    {
        public AuthorizationStatus Status { get; set; }

        public string Base64QR { get; set; }
    }
}
