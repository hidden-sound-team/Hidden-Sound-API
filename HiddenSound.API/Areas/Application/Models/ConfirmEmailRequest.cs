using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Models
{
    public class ConfirmEmailRequest
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
