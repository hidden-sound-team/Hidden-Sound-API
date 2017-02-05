using System;
using System.Linq;

namespace HiddenSound.API.Areas.API.Models.Responses
{
    public class TransactionCreateResponse
    {
        public string AuthorizationCode { get; set; }

        public string Base64QR { get; set; }
    }
}
