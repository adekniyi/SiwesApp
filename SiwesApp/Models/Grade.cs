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
        public int GradeValue { get; set; }
        public int LogBookId { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public virtual IndustrialSupervisor IndustrialSupervisor { get; set; }
        public virtual LogBook LogBook { get; set; }
    }
}
