using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HiddenSound.API.Identity
{
    public class HiddenSoundRole : IdentityRole<int>
    {
        public HiddenSoundRole()
        {

        }

        public HiddenSoundRole(string roleName) : base(roleName)
        {

        }
    }
}
