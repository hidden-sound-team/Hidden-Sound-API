using HiddenSound.API.Identity;
using OpenIddict.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.OpenIddict
{
    public class HSOpenIddictApplication : OpenIddictApplication<Guid, HSOpenIddictAuthorization, HSOpenIddictToken>
    {
        [ForeignKey("User")]
        public Guid? UserId { get; set; }

        public string Description { get; set; }

        public string WebsiteUri { get; set; }

        public HiddenSoundUser User { get; set; }
    }
}
