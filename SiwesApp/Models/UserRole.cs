using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SiwesApp.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}