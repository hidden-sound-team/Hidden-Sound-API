﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Shared.Models
{
    [Table("User")]
    public class User
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Password_Hash")]
        public string Password { get; set; }

        [Column("Is_Developer")]
        public bool IsDeveloper { get; set; }

        [Column("Is_Verified")]
        public bool IsVerified { get; set; }

        public EmailVerification EmailVerification { get; set; }
    }
}