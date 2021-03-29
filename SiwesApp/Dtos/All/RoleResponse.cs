using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.All
{
    public class RoleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RoleDescription { get; set; }
        public int UserType { get; set; }
        public int NumberOfUsersAssignedToRole { get; set; }
    }
}
