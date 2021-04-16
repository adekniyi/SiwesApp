using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int GraderId { get; set; }
        public int ObtainedGrade { get; set; }
        public int ObtainableGrade { get; set; }
        public int LogBookId { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public virtual IndustrialSupervisor IndustrialSupervisor { get; set; }
        public virtual LogBook LogBook { get; set; }
    }
}
