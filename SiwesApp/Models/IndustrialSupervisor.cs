using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Models
{
    public class IndustrialSupervisor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IndustrialSupervisorId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string SectionOfWork { get; set; }
    }
}
