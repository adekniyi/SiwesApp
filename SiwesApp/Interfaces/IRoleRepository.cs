using SiwesApp.Dtos.All;
using SiwesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    public interface IRoleRepository
    {
        public Task<ToRespond> CreateRoles(List<Role> roles);
        public Task<ToRespond> GetRoles(int id);
        public Task<ToRespond> UpdateRoles(List<RoleResponse> roles);
        public Task<ToRespond> DeleteRoles(List<RoleResponse> roles);
        public Task<ToRespond> AssignRolesToUser(RoleUserAssignmentRequest roleAssignmentRequest);
    }
}
