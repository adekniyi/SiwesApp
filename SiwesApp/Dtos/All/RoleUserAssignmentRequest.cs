using SiwesApp.Dtos.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.All
{
    public class RoleUserAssignmentRequest
    {
        public List<UserToReturn> Users { get; set; }
        public List<RoleResponse> Roles { get; set; }
    }
}
