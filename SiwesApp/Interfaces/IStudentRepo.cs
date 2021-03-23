using SiwesApp.Dtos.All;
using SiwesApp.Dtos.StudentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    interface IStudentRepo
    {
        public Task<ToRespond> CreateStudent(StudentRequest studentRequest);
    }
}
