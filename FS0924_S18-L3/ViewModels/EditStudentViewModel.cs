using System.ComponentModel.DataAnnotations;

namespace FS0924_S18_L3.ViewModels
{
    public class EditStudentViewModel
    {
        public Guid Id { get; set; }

        [StringLength(
            50,
            ErrorMessage = "Name must contains from 2 to 50 chars!",
            MinimumLength = 2
        )]
        public string? Name { get; set; }

        [StringLength(
            50,
            ErrorMessage = "Surname must contains from 2 to 50 chars!",
            MinimumLength = 2
        )]
        public string? Surname { get; set; }

        [Required]
        public DateTime BirthdayDate { get; set; }

        [StringLength(
            300,
            ErrorMessage = "Email must contains from 6 to 50 chars!",
            MinimumLength = 6
        )]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
