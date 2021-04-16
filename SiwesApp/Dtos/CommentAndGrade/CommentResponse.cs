using SiwesApp.Dtos.IndustrialSupervisorDto;
using SiwesApp.Dtos.LecturerDto;
using SiwesApp.Dtos.StudentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.CommentAndGrade
{
    public class CommentResponse
    {
        public int CommentId { get; set; }
        public string CommentDetail { get; set; }
        public  LecturerResponse Lecturer { get; set; }
        public  IndustrialSupervisorResponse IndustrialSupervisor { get; set; }
        public  LogBookResponse LogBook { get; set; }
    }
}
