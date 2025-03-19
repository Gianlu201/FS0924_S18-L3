using System.ComponentModel.DataAnnotations;

namespace FS0924_S18_L3.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
