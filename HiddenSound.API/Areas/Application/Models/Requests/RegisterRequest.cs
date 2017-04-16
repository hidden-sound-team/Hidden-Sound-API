using System.ComponentModel.DataAnnotations;

namespace HiddenSound.API.Areas.Application.Models.Requests
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Language { get; set; } = "EN";

        public string Timezone { get; set; } = "-05:00";

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
