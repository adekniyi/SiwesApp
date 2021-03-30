using AutoMapper;
using SiwesApp.Dtos.All;
using SiwesApp.Dtos.LecturerDto;
using SiwesApp.Dtos.SiwesAdmin;
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

            CreateMap<SiwesAdmin, SiwesAdminResponse>();
            CreateMap<SiwesAdminResponse, SiwesAdmin>();

            CreateMap<SiwesCoordinatorRequest, SiwesCoordinator>();
            CreateMap<SiwesCoordinator, SiwesCoordinatorResponse>();
            CreateMap<SiwesCoordinatorResponse, SiwesCoordinator>();


            CreateMap<LecturerRequest, Lecturer>();
            CreateMap<Lecturer, LecturerResponse>();
            CreateMap<LecturerResponse, Lecturer>();


            CreateMap<RoleRequest, Role>();
            CreateMap<Role, RoleResponse>();
            CreateMap<RoleResponse, Role>().ForMember(a => a.Id, b => b.Ignore()).ForMember(a1 => a1.DeletedAt, b1 => b1.Ignore()).ForMember(a2 => a2.CreatedAt, b2 => b2.Ignore());

        }
    }
}
