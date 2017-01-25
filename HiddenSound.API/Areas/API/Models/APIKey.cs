using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.API.Models
{
    [Table("API_Key")]
    public class APIKey
    {
        public int ID { get; set; }

        [Column("Public_Key")]
        public string PublicKey { get; set; }

        [Column("Private_Key")]
        public string PrivateKey { get; set; }
    }
}
