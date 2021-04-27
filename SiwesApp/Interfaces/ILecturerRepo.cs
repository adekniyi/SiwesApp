using SiwesApp.Dtos.All;
using SiwesApp.Dtos.CommentAndGrade;
using SiwesApp.Dtos.LecturerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    public interface ILecturerRepo
    {
        public Task<ToRespond> CreateLecturer(LecturerRequest lecturerRequest);
        public Task<ToRespond> GetAllLecturers();
        public Task<ToRespond> GetOneLecturer(int lecturerId);
        public Task<ToRespond> UpdateLecturer(int lecturerId, LecturerRequest lecturerRequest);

    }
}
