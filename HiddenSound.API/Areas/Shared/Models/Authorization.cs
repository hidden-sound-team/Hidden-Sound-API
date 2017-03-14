using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HiddenSound.API.Identity;
using HiddenSound.API.OpenIddict;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace HiddenSound.API.Areas.Shared.Models
{
    public class Authorization
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        
        [Required]
        public AuthorizationStatus Status { get; set; }

        [Required]
        public DateTime ExpiresOn { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        
        [ForeignKey("Application")]
        public Guid? ApplicationId { get; set; }

        public HiddenSoundUser User { get; set; }

        public HSOpenIddictApplication Application { get; set; }
    }
}
