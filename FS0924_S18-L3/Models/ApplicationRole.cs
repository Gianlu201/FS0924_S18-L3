using Microsoft.AspNetCore.Identity;

namespace FS0924_S18_L3.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> ApplicationUserRole { get; set; }
    }
}
