using SiwesApp.Dtos.LecturerDto;
using SiwesApp.Dtos.StudentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.SiwesCoOrdinatotDto
{
    public class AssignStudentToLecturerResponse
    {
        public int AssignStudentToLecturerId { get; set; }
        public List<StudentResponse> Student { get; set; }
        public int LecturerId { get; set; }
        public int? IndustrialSupervisorId { get; set; }
        public LecturerResponse Lecturer { get; set; }

    }
}
