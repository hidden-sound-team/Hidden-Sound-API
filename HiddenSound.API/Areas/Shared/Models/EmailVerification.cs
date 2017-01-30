using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Shared.Models
{
    public class EmailVerification
    {
        public int ID { get; set; }

        [Column("Verification_Code")]
        public string VerificationCode { get; set; }

        [Column("User_ID")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        [Column("Created_On")]
        public DateTime CreatedOn { get; set; }
    }
}
