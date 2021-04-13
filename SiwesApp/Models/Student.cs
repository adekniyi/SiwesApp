using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public int MatricNumber { get; set; }
        public string Department { get; set; }
        public string Level { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string Gender { get; set; }
        public int? EligiblityStatus { get; set; }
        public DateTimeOffset DOB { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string Address { get; set; }
        public int? PlacementId { get; set; }
        public virtual User User { get; set; }
        public virtual Placement Placement { get; set; }
        public virtual AssignStudentToLecturer AssignStudentToLecturer { get; set; }
    }
}
