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
        public Task<ToRespond> CommentOnLogBook(int logBookId,CommentRequest commentRequest);
        public Task<ToRespond> GradeLogBook(int logBookId, GradeRequest gradeRequest);
        public Task<ToRespond> GetACommenttedLogBook(int commentId);
        public Task<ToRespond> GetAGradedLogBook(int gradeId);
        public Task<ToRespond> GetAllStudentCommenttedLogBook();
        public Task<ToRespond> GetAllStudentGradedLogBook();

    }
}
