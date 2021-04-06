using SiwesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.All
{
    public class UserDetails
    {
        public string Token { get; set; }
        public User User { get; set; }
        public object userProfile { get; set; }
    }
}
