using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.StudentDto
{
    public class PlacementResponse
    {
        public int PlacementId { get; set; }
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MatricNumber { get; set; }
        public int RegistrationNumber { get; set; }
        public string Department { get; set; }
        public string Programm { get; set; }
        public string Level { get; set; }
        public string OfferLetter { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string SectionOfWork { get; set; }
        public string EmailAddressOfCompany { get; set; }

    }
}
