using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Models.Requests
{
    public class DevicePutRequest
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
    }
}
