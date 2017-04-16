using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HiddenSound.API.Identity
{
    public class HiddenSoundUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Language { get; set; } = "EN";

        public string Timezone { get; set; } = "-05:00";
    }
}
