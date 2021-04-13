using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Models
{
    public class AssignStudentToLecturer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AssignStudentToLecturerId { get; set; }
        public int StudentId { get; set; }
        public int LecturerId { get; set; }
        public int? IndustrialSupervisorId { get; set; }
        public virtual List<Student> Student { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public virtual IndustrialSupervisor IndustrialSupervisor { get; set; }

    }
}
