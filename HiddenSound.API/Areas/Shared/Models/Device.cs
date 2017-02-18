using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Identity;
using Newtonsoft.Json;

namespace HiddenSound.API.Areas.Shared.Models
{
    [Table("Device")]
    public class Device
    {
        [Column("ID")]
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("IMEI")]
        [Required]
        public string IMEI { get; set; }

        [Column("UserID")]
        [ForeignKey("User")]
        public int? UserID { get; set; }

        [JsonIgnore]
        public HiddenSoundUser User { get; set; }
    }
}
