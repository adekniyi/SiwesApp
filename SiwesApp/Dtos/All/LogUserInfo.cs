using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.All
{
    public class LogUserInfo
    {
        public int UserTypeId { get; set; }
        public string UserId { get; set; }
        public List<Claim> Roles { get; set; }
    }
}
