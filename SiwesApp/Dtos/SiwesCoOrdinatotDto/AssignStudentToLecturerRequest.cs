using SiwesApp.Dtos.LecturerDto;
using SiwesApp.Dtos.StudentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.SiwesCoOrdinatotDto
{
    public class AssignStudentToLecturerRequest
    {
        public List<int> Students { get; set; }
        public int Lecturer { get; set; } 
        public int IndustrialSupervisorId { get; set; } 
    }
}
