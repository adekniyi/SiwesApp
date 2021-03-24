using SiwesApp.Dtos.All;
using SiwesApp.Dtos.LecturerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    interface ILecturerRepo
    {
        public Task<ToRespond> CreateLecturer(LecturerRequest lecturerRequest);

    }
}
