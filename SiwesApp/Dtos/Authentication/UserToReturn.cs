using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.All
{
    public class UserToReturn
    {
        public int Id { get; set; }
        public int UserType { get; set; } 
        public int UserTypeId { get; set; } 
        public bool EmailConfirmed { get; set; }
    }
}
