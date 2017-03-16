using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HiddenSound.API.Areas.Mobile.Models.Requests
{
    public class AuthorizationAuthorizeRequest
    {
        [Required]
        public string AuthorizationCode { get; set; }

        [Required]
        public string IMEI { get; set; }
    }
}
