using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public int UserType { get; set; }
        public int UserTypeId { get; set; } 
        public bool Deleted { get; set; }
        public string ShortToken { get; set; }
        public string LongToken { get; set; }
        public DateTimeOffset? LastLoginDateTime { get; set; }
        public DateTimeOffset? SecondToLastLoginDateTime { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
    }
}
