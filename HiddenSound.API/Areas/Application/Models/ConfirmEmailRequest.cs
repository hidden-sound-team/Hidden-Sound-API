using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Models
{
    public class ConfirmEmailRequest
    {
        public int UserID { get; set; }

        public string Code { get; set; }
    }
}
