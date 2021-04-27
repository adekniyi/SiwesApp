using SiwesApp.Dtos.All;
using SiwesApp.Dtos.SiwesCoOrdinatotDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    public interface ISiwesCoordinatorRepo
    {
        public Task<ToRespond> CreateSiwesCo(SiwesCoordinatorRequest siwesCoordinatorRequest);
        public Task<ToRespond> GetAllSiwesCos();
        public Task<ToRespond> GetOneSiwesCo(int siwesCoId);
        public Task<ToRespond> MakePlacementEligible(int studentId);
        public Task<ToRespond> RejectPlacement(int studentId);
        public Task<ToRespond> AssignStudentToLecturer(AssignStudentToLecturerRequest assignStudentToLecturer);
        public Task<ToRespond> GetStudentLogBooks(int studentId);
        public Task<ToRespond> GetStudentLogBook(int studentId, int logBookId);
        public Task<ToRespond> UpdateSiwesCo(int siwesCoId,SiwesCoordinatorRequest siwesCoordinatorRequest);

    }
}
