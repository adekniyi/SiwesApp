using Microsoft.AspNetCore.Identity;

namespace SiwesApp.Models
{
    public class UserRole : IdentityUser<int>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}