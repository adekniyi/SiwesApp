using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiwesApp.Data;
using SiwesApp.Dtos.All;
//using SiwesApp.Dtos.Authentication;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public RoleRepository(RoleManager<Role> roleManager, ApplicationDataContext dataContext, IMapper mapper, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _dataContext = dataContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ToRespond> AssignRolesToUser(RoleUserAssignmentRequest roleAssignmentRequest)
        {
            if (roleAssignmentRequest.Users == null || roleAssignmentRequest.Roles == null || roleAssignmentRequest == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            //CHECK THE LIST OF ROLES TO ASSIGN FOR AUTHENTICITY
            //var listOfRolesToAssign = new List<string>();
            var listOfRolesToReturn = new List<Role>();

            foreach (var h in roleAssignmentRequest.Roles)
            {
                var roleDetail = await _roleManager.FindByIdAsync(Convert.ToString(h.Id));
                if (roleDetail == null)
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotFound,
                        StatusMessage = Helpers.StatusMessageNotFound
                    };
                }

                //listOfRolesToAssign.Add(roleDetail.Name);
                listOfRolesToReturn.Add(roleDetail);
            }

            var userRolesToReturn = new List<UserAndRoleResponse>();

            foreach (var z in roleAssignmentRequest.Users)
            {
                var userDetail = await _userManager.FindByIdAsync(Convert.ToString(z.Id));
                if (userDetail == null)
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotFound,
                        StatusMessage = Helpers.StatusMessageNotFound
                    };
                }

                //DELETE THE USER'S OLD ROLES
                var usersRoles = await _userManager.GetRolesAsync(userDetail);
                var iResult = await _userManager.RemoveFromRolesAsync(userDetail, usersRoles.AsEnumerable());
                if (!iResult.Succeeded)
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotSucceeded,
                        StatusMessage = "Not Succeded"
                    };
                }

                var listOfRolesToAssign = listOfRolesToReturn.Select(a => a.Name);
                //UPDATE THE USER'S ROLES WITH THIS CURRENT INCOMING ROLES
                //foreach (var roleDett in listOfRolesToAssign)
                //{
                //    //CHECK TO SEE IF ANYBODY HOLDS THAT ROLE (APART FROM AllStaff ROLE)...IF ANY OVERWRITE IT

                //}

                try
                {
                    var result = await _userManager.AddToRolesAsync(userDetail, listOfRolesToAssign);
                    if (!result.Succeeded)
                    {
                        return new ToRespond()
                        {
                            StatusCode = Helpers.NotSucceeded,
                            StatusMessage = "Not succeeded"
                        };
                    }
                }
                catch (Exception)
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.RoleAssignmentError,
                        StatusMessage = Helpers.StatusMessageRoleAssignmentError
                    };
                }


                userRolesToReturn.Add(new UserAndRoleResponse()
                {
                    User = _mapper.Map<UserToReturn>(userDetail),
                    Roles = _mapper.Map<List<RoleResponse>>(listOfRolesToReturn)
                });
            }

            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                ObjectValue = userRolesToReturn,
                StatusMessage = Helpers.StatusMessageSuccess
            };
        }

        public async Task<ToRespond> CreateRoles(List<Role> roles)
        {
            if (roles == null || roles.Any(a => string.IsNullOrWhiteSpace(a.Name)))
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            var rolesToReturn = new List<Role>();
            var dbTransaction = await _dataContext.Database.BeginTransactionAsync();
            foreach (var roleType in roles)
            {
                if (roleType.UserType != Helpers.SiwesAdmin && roleType.UserType != Helpers.SiwesCoordinator && roleType.UserType != Helpers.Lecturer && roleType.UserType != Helpers.Student && roleType.UserType != Helpers.IndustrialSupervisor)
                {
                    await dbTransaction.RollbackAsync();
                    return new ToRespond()
                    {
                        StatusCode = 13,
                        StatusMessage = "Invalid Role Type"
                    };
                }

                roleType.RoleName = roleType.Name;
                var result = await _roleManager.CreateAsync(roleType);
                if (!result.Succeeded)
                {
                    await dbTransaction.RollbackAsync();
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotSucceeded,
                        StatusMessage = "Not Succeeded"
                    };
                }

                rolesToReturn.Add(roleType);
            }

            await dbTransaction.CommitAsync();
            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                ObjectValue = rolesToReturn,
                StatusMessage = Helpers.StatusMessageSuccess
            };
        }

        public async Task<ToRespond> DeleteRoles(List<RoleResponse> roles)
        {
            if (roles == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            var rolesToReturn = new List<Role>();
            var dbTransaction = await _dataContext.Database.BeginTransactionAsync();
            foreach (var t in roles)
            {
                var roleDetail = await _roleManager.FindByIdAsync(Convert.ToString(t.Id));
                if (roleDetail == null)
                {
                    await dbTransaction.RollbackAsync();
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotFound,
                        StatusMessage = Helpers.StatusMessageNotFound
                    };
                }

                var result = await _roleManager.DeleteAsync(roleDetail);
                if (!result.Succeeded)
                {
                    await dbTransaction.RollbackAsync();
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotSucceeded,
                        StatusMessage = "Not Succeeded"
                    };
                }

                rolesToReturn.Add(roleDetail);
            }

            await dbTransaction.CommitAsync();
            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                ObjectValue = rolesToReturn,
                StatusMessage = Helpers.StatusMessageSuccess
            };
        }

        public async Task<ToRespond> GetRoles(int id)
        {
            var role = await _roleManager.Roles.Where(c => c.Id == id)
                .Include(a => a.UserRoles).FirstOrDefaultAsync();

            if (role == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                ObjectValue = role,
                StatusMessage = Helpers.StatusMessageSuccess
            };
        }

        public async Task<ToRespond> UpdateRoles(List<RoleResponse> roles)
        {
            if (roles == null || roles.Any(a => string.IsNullOrWhiteSpace(a.Name)))
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            var rolesToReturn = new List<Role>();
            var dbTransaction = await _dataContext.Database.BeginTransactionAsync();
            foreach (var t in roles)
            {
                if (t.UserType != Helpers.Lecturer && t.UserType != Helpers.SiwesAdmin && t.UserType != Helpers.Student && t.UserType != Helpers.SiwesCoordinator)
                {
                    await dbTransaction.RollbackAsync();
                    return new ToRespond()
                    {
                        StatusCode = Helpers.InvalidUserType,
                        StatusMessage = Helpers.StatusMessageInvalidUserType
                    };
                }

                var roleDetail = await _roleManager.FindByIdAsync(Convert.ToString(t.Id));
                if (roleDetail == null)
                {
                    await dbTransaction.RollbackAsync();
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotFound,
                        StatusMessage = Helpers.StatusMessageNotFound
                    };
                }

                //var roleToUpdate = _mapper.Map(t, roleDetail);
                //roleToUpdate.ModifiedAt = DateTimeOffset.Now;
                roleDetail.Name = t.Name;
                roleDetail.RoleDescription = t.RoleDescription;
                roleDetail.UserType = t.UserType;
                roleDetail.RoleName = t.Name;
                roleDetail.ModifiedAt = DateTimeOffset.Now;

                var result = await _roleManager.UpdateAsync(roleDetail);
                if (!result.Succeeded)
                {
                    await dbTransaction.RollbackAsync();
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotSucceeded,
                        StatusMessage = "Not Succeeded"
                    };
                }

                rolesToReturn.Add(roleDetail);
            }

            await dbTransaction.CommitAsync();
            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                ObjectValue = rolesToReturn,
                StatusMessage = Helpers.StatusMessageSuccess
            };
        }
    }
}
