using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Models.Requests
{
    public class ApplicationInfoDeleteRequest
    {
        [Required]
        public string ClientId { get; set; }
    }
}
