using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HiddenSound.API.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace HiddenSound.API.Areas.Shared.Models
{
    public class Transaction
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string AuthorizationCode { get; set; }
        
        [Required]
        public TransactionStatus Status { get; set; }
        
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        
        [ForeignKey("Vendor")]
        public Guid? VendorId { get; set; }
        
        [Required]
        public DateTime ExpiresOn { get; set; }

        [NotMapped]
        public string Base64QR { get; set; }

        public HiddenSoundUser User { get; set; }

        public HiddenSoundUser Vendor { get; set; }
    }
}
