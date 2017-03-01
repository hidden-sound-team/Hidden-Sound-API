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
    public class Device
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string IMEI { get; set; }
        
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        
        public HiddenSoundUser User { get; set; }
    }
}
