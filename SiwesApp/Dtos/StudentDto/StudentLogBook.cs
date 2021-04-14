using SiwesApp.Dtos.LecturerDto;
using SiwesApp.Dtos.SiwesCoOrdinatotDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.StudentDto
{
    public class StudentLogBook
    {
        public int StudentId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int MatricNumber { get; set; }
        public string Department { get; set; }
        public string Level { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset DOB { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string Address { get; set; }
        public int EligiblityStatus { get; set; }
        public PlacementResponse Placement { get; set; }

        public List<LogBookResponse> LogBook { get; set; }
        public LecturerResponse Lecturer { get; set; }
    }
}
