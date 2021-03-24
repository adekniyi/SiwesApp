using AutoMapper;
using SiwesApp.Dtos.LecturerDto;
using SiwesApp.Dtos.SiwesCoOrdinatotDto;
using SiwesApp.Dtos.StudentDto;
using SiwesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<StudentRequest, Student>();
            CreateMap<Student, StudentResponse>();
            CreateMap<StudentResponse, Student>();


            CreateMap<SiwesCoordinatorRequest, SiwesCoordinator>();
            CreateMap<SiwesCoordinator, SiwesCoordinatorResponse>();
            CreateMap<SiwesCoordinatorResponse, SiwesCoordinator>();


            CreateMap<LecturerRequest, Lecturer>();
            CreateMap<Lecturer, LecturerResponse>();
            CreateMap<LecturerResponse, Lecturer>();
        }
    }
}
