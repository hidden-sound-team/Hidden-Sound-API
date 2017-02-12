using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HiddenSound.API.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace HiddenSound.API.Areas.Shared.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        [Column("ID")]
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("Authorization_Code")]
        [Required]
        [MaxLength(50)]
        public string AuthorizationCode { get; set; }

        [Column("Status")]
        [Required]
        public TransactionStatus Status { get; set; }

        [Column("User_ID")]
        [ForeignKey("User")]
        public int? UserID { get; set; }

        [Column("Vendor_ID")]
        [ForeignKey("Vendor")]
        public int? VendorID { get; set; }

        [Column("Expires_On")]
        [Required]
        public DateTime ExpiresOn { get; set; }

        [NotMapped]
        public string Base64QR { get; set; }

        [JsonIgnore]
        public HiddenSoundUser User { get; set; }

        [JsonIgnore]
        public HiddenSoundUser Vendor { get; set; }
    }
}
