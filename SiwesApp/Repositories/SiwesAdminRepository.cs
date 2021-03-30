using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiwesApp.Data;
using SiwesApp.Dtos;
using SiwesApp.Dtos.All;
using SiwesApp.Dtos.Authentication;
using SiwesApp.Dtos.SiwesAdmin;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Repositories
{
    public class SiwesAdminRepository : ISiwesAdminRepo
    {
        private readonly ApplicationDataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGlobalRepository _globalRepository;
        private readonly IConfiguration _configuration;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationRepo _authenticationRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly RoleManager<Role> _roleManager;


        public SiwesAdminRepository(ApplicationDataContext dataContext, UserManager<User> userManager,
            SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment, IGlobalRepository globalRepository, IAuthenticationRepo authenticationRepository,
            IMapper mapper, RoleManager<Role> roleManager,IRoleRepository roleRepository)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _globalRepository = globalRepository;
            _mapper = mapper;
            _authenticationRepository = authenticationRepository;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
        }
        public async Task<ToRespond> CreateSiwesAdmin(SiwesAdminRequest siwesAdminRequest)
        {
            if (siwesAdminRequest == null || string.IsNullOrWhiteSpace(siwesAdminRequest.EmailAddress))
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = "Super Admin Information is Null"
                };
            }

            //GET SUPERADMIN ROLE
            var superAdminRole = await _roleManager.FindByNameAsync(Helpers.SiwesAdminRole);
            if (superAdminRole == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = "Super Admin Role is Not Found"
                };
            }

            var siwesAdminRoleForAssignment = new RoleResponse()
            {
                Id = superAdminRole.Id
            };

            if (await SiwesAdminExists(siwesAdminRequest.EmailAddress))
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectExists,
                    StatusMessage = "One or more of the provided resources already exist(s)!"
                };
            }

            var siwesAdmin = new SiwesAdmin
            {
                EmailAddress = siwesAdminRequest.EmailAddress,
                FirstName = siwesAdminRequest.FirstName,
                LastName = siwesAdminRequest.LastName,
            };

            _globalRepository.Add(siwesAdmin);
            var saveResult = await _globalRepository.SaveAll();
            if (saveResult.HasValue)
            {
                if (!saveResult.Value)
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.SaveNoRowAffected,
                        StatusMessage = "SuperAdmin Information Could Not Save"
                    };
                }

                var user = new User
                {
                    UserName = siwesAdmin.EmailAddress,
                    Email = siwesAdmin.EmailAddress,
                    UserTypeId = siwesAdmin.SiwesAdminId,
                    UserType = Helpers.SiwesAdmin,
                };
                var result = _userManager.CreateAsync(user, siwesAdminRequest.Password).Result;
                if (result.Succeeded)
                {
                    siwesAdmin.UserId = user.Id;
                    var siwesAdminUpdateResult = _globalRepository.Update(siwesAdmin);
                    if (!siwesAdminUpdateResult)
                    {
                        return new ToRespond()
                        {
                            StatusCode = Helpers.NotSucceeded,
                            StatusMessage = "Error Occured while saving Staff Information"
                        };
                    }

                    var siwesAdminUpdateSaveResult = await _globalRepository.SaveAll();
                    if (!siwesAdminUpdateSaveResult.HasValue)
                    {
                        return new ToRespond()
                        {
                            StatusCode = Helpers.SaveError,
                            StatusMessage = "Error Occured while saving Super Admin Information"
                        };
                    }

                    if (!siwesAdminUpdateSaveResult.Value)
                    {
                        return new ToRespond()
                        {
                            StatusCode = Helpers.SaveNoRowAffected,
                            StatusMessage = "Error Occured while saving Super Admin Information"
                        };
                    }

                    //IF NO ROLE CAME WITH THE SUPERADMIN REGISTER REQUEST ASSIGN DEFAULT ROLE OF SUPERADMIN TO THAT USER
                    var userRole = new List<RoleResponse>()
                            {
                                siwesAdminRoleForAssignment
                            };

                    var assignmentResult = await _roleRepository.AssignRolesToUser(new RoleUserAssignmentRequest()
                    {
                        Users = new List<UserToReturn>() {
                                new UserToReturn()
                                {
                                    Id = user.Id
                                }
                            },
                        Roles = userRole
                    });
                    if (assignmentResult.StatusCode == Helpers.Success)
                    {
                        var userTokenVal = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        string hashedEmail = GetHashedEmail(user.Email);
                        string fullToken = userTokenVal + "#" + hashedEmail;
                        var emailVerificationLink = _authenticationRepository.GetUserEmailVerificationLink(fullToken);
                        if (emailVerificationLink == null)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.ObjectNull,
                                StatusMessage = "Could not generate Email Verification Link"
                            };
                        }
                        else
                        {
                            var siwesAdminToReturn = await GetOneSiwesAdmin(siwesAdmin.SiwesAdminId);

                            return new ToRespond()
                            {
                                StatusCode = Helpers.Success,
                                ObjectValue = (User)siwesAdminToReturn.ObjectValue,
                                StatusMessage = "SuperAdmin Created Successfully!!!"
                            };
                        }
                    }
                    else
                    {
                        return new ToRespond()
                        {
                            StatusCode = Helpers.NotSucceeded,
                            StatusMessage = "Error Occured while saving SuperAdmin Information"
                        };
                    }
                }
                else
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotSucceeded,
                        StatusMessage = "Error Occured while saving SuperAdmin Information"
                    };
                }
            }
            else
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.SaveError,
                    StatusMessage = "Error Occured while saving SuperAdmin Information"
                };
            }
        }

        private async Task<bool> SiwesAdminExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.Email.ToUpper() == username.ToUpper());
        }

        public async Task<ToRespond> GetOneSiwesAdmin(int id)
        {
            var superAdmin = await _userManager.Users
                                             .Where(a => (a.UserTypeId == id) && (a.UserType == Helpers.SiwesAdmin) && (!a.Deleted))
                                             .Include(c => c.UserRoles)
                                             .ThenInclude(d => d.Role)
                                             .ToListAsync();
            if (superAdmin == null)
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
                ObjectValue = superAdmin.FirstOrDefault(),
                StatusMessage = Helpers.StatusMessageSuccess
            };
        }

        private string GetHashedEmail(string emailVal)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(emailVal));
        }
    }
}
