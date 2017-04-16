using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Identity;
using HiddenSound.API.OpenIddict;

namespace HiddenSound.API.Areas.Shared.Models
{
    public class AuthorizedApplication
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }

        [ForeignKey("Application")]
        public Guid? ApplicationId { get; set; }

        public virtual HiddenSoundUser User { get; set; }

        public virtual HSOpenIddictApplication Application { get; set; }
    }
}
