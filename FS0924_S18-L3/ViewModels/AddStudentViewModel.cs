using System.ComponentModel.DataAnnotations;

namespace FS0924_S18_L3.ViewModels
{
    public class AddStudentViewModel
    {
        [Required]
        [StringLength(
            50,
            ErrorMessage = "Name must contains from 2 to 50 chars!",
            MinimumLength = 2
        )]
        public required string Name { get; set; }

        [Required]
        [StringLength(
            50,
            ErrorMessage = "Surname must contains from 2 to 50 chars!",
            MinimumLength = 2
        )]
        public required string Surname { get; set; }

        [Required]
        public DateTime BirthdayDate { get; set; }

        [StringLength(
            300,
            ErrorMessage = "Email must contains from 6 to 50 chars!",
            MinimumLength = 6
        )]
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
