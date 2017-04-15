using System;
using System.Linq;
using HiddenSound.API.Areas.Shared.Models;

namespace HiddenSound.API.Areas.API.Models.Responses
{
    public class AuthorizationCreateResponse
    {
        public string AuthorizationCode { get; set; }

        public string Base64QR { get; set; }

        public DateTime ExpiresOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public AuthorizationStatus Status { get; set; }
    }
}
