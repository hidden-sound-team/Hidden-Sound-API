﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Models.Responses
{
    public class UserInfoResponse
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Language { get; set; }

        public string Timezone { get; set; }
    }
}
