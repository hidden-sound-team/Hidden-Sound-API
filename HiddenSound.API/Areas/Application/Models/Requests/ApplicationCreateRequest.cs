using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Models.Requests
{
    public class ApplicationCreateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string RedirectUri { get; set; }
    }
}
