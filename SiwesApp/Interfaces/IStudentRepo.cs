using SiwesApp.Dtos.All;
using SiwesApp.Dtos.StudentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    public interface IStudentRepo
    {
        public Task<ToRespond> CreateStudent(StudentRequest studentRequest);
        public Task<ToRespond> GetAllStudents();
        public Task<ToRespond> GetOneStudent(int studentId);
        public Task<ToRespond> StudentPlacement(PlacementRequestDto placementRequest);
    }
}
