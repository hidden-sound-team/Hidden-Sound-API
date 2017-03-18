using System.ComponentModel.DataAnnotations;

namespace HiddenSound.API.Areas.Application.Models.Requests
{
    public class ConfirmEmailRequest
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
