using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace HiddenSound.API.Auth
{
    public class HmacSha256Options : AuthenticationOptions
    {
        public ulong MaxRequestAgeInSeconds { get; set; }

        public HmacSha256Options()
        {
            AuthenticationScheme = "Shared";
            MaxRequestAgeInSeconds = 300;
        }
    }
}
