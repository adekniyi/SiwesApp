using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.StudentDto
{
    public class PlacementRequestDto
    {
        public string FirstName { get; set; }
        public int MatricNumber { get; set; }
        public string Department { get; set; }
        public int RegistrationNumber { get; set; }
        public string Level { get; set; }
        public string LastName { get; set; }
        public string Programm { get; set; }
        public IFormFile OfferLetter { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string SectionOfWork { get; set; }
        public string EmailAddressOfCompany { get; set; }

    }
}
