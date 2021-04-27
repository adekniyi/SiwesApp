using SiwesApp.Dtos.All;
using SiwesApp.Dtos.CommentAndGrade;
using SiwesApp.Dtos.StudentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    public interface ILogBookRepository
    {
        public Task<ToRespond> FillLogBook(LogBookRequest logBookRequest);
        public Task<ToRespond> CommentOnLogBook(int logBookId, CommentRequest commentRequest);
        public Task<ToRespond> GradeLogBook(int logBookId, GradeRequest gradeRequest);
        public Task<ToRespond> GetACommenttedLogBook(int commentId);
        public Task<ToRespond> GetAGradedLogBook(int gradeId);
        public Task<ToRespond> GetAllStudentCommenttedLogBook();
        public Task<ToRespond> GetAllStudentGradedLogBook();
        public Task<ToRespond> UpdateLogBook(int logBookId,LogBookRequest logBookRequest);
    }
}
