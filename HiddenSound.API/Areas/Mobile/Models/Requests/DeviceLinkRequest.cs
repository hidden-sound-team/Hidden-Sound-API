using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Mobile.Models.Requests
{
    public class DeviceLinkRequest
    {
        [Required]
        public string IMEI { get; set; }
    }
}
