using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HiddenSound.API.Areas.Shared.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("Authorization_Code")]
        public string AuthorizationCode { get; set; }

        [Column("Status")]
        public TransactionStatus Status { get; set; }

        [Column("Base64_QR")]
        public string Base64QR { get; set; }

        [Column("User_ID")]
        public int UserID { get; set; }

        [Column("Vendor_ID")]
        public int VendorID { get; set; }

        [Column("Expires_On")]
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public User Vendor { get; set; }
    }
}
