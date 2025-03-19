using System.ComponentModel.DataAnnotations;

namespace FS0924_S18_L3.Models
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Surname { get; set; }

        [Required]
        public DateTime BirthdayDate { get; set; }

        [MaxLength(300)]
        public string? Email { get; set; }
    }
}
