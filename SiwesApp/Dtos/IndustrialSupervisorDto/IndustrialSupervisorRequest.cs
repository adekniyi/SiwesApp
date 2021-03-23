using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.IndustrialSupervisorDto
{
    public class IndustrialSupervisorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string SectionOfWork { get; set; }
        //public string CompanyEmail { get; set; }
    }
}
