using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiwesApp.Models
{
    public class Role : IdentityRole<int>
    {
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int UserType { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
    }
}