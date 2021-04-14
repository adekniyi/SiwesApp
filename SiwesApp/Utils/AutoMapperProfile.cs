using AutoMapper;
using SiwesApp.Dtos.All;
using SiwesApp.Dtos.IndustrialSupervisorDto;
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

            CreateMap<IndustrialSupervisorRequest, IndustrialSupervisor>();
            CreateMap<IndustrialSupervisor, IndustrialSupervisorResponse>();
            CreateMap<IndustrialSupervisorResponse, IndustrialSupervisor>();

            CreateMap<LecturerRequest, Lecturer>();
            CreateMap<Lecturer, LecturerResponse>();
            CreateMap<LecturerResponse, Lecturer>();

            CreateMap<PlacementRequestDto, Placement>();
            CreateMap<Placement, PlacementResponse>();
            CreateMap<PlacementResponse, Placement>();

            CreateMap<AssignStudentToLecturerRequest, AssignStudentToLecturer>();
            CreateMap<AssignStudentToLecturer, AssignStudentToLecturerResponse>();
            CreateMap<AssignStudentToLecturerResponse, AssignStudentToLecturer>();

            CreateMap<LogBookRequest, LogBook>();
            CreateMap<LogBook, LogBookResponse>();
            //CreateMap<LogBookResponse, LogBook>();

            //CreateMap<User, UserDetails>();
            CreateMap<User, UserToReturn>();
            CreateMap<User, UserDetails>();
            CreateMap<UserDetails, UserLoginResponse>();

            CreateMap<RoleRequest, Role>();
            CreateMap<Role, RoleResponse>();
            CreateMap<RoleResponse, Role>().ForMember(a => a.Id, b => b.Ignore());

        }
    }
}
