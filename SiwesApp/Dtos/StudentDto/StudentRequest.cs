using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.StudentDto
{
    public class StudentRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile StudentPicture { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset DOB { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string Address { get; set; }

    }
}
