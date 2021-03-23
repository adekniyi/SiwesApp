using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.SiwesCoOrdinatotDto
{
    public class SiwesCoordinatorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string Department { get; set; }

    }
}
