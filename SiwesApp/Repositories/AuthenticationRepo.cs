using Microsoft.AspNetCore.Identity;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using SiwesApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using SiwesApp.Dtos.All;
using Microsoft.EntityFrameworkCore;
using SiwesApp.Dtos.StudentDto;

namespace SiwesApp.Repositories
{
    public class AuthenticationRepo : IAuthenticationRepo
    {
        private readonly ApplicationDataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IGlobalRepository _globalRepository;

        public AuthenticationRepo(ApplicationDataContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,IMapper mapper, IGlobalRepository globalRepository)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _mapper = mapper;
            _globalRepository = globalRepository;
        }
        public async Task<string> GenerateJwtToken(User user, string secretKey)
        {
            //Get User info
            var claims = new List<Claim>();


            if (user.UserType == Helpers.SiwesAdmin)
            {
                claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(Helpers.ClaimType_UserEmail, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserTypeId.ToString()),
                    new Claim(Helpers.ClaimType_UserType, user.UserType.ToString())
                };
            }
            else if (user.UserType == Helpers.SiwesCoordinator)
            {
                claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(Helpers.ClaimType_UserEmail, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserTypeId.ToString()),
                    new Claim(Helpers.ClaimType_UserType, user.UserType.ToString()),
                };
            }
            else if (user.UserType == Helpers.Lecturer)
            {
                claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(Helpers.ClaimType_UserEmail, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserTypeId.ToString()),
                    new Claim(Helpers.ClaimType_UserType, user.UserType.ToString())
                };
            }
            else if (user.UserType == Helpers.IndustrialSupervisor)
            {
                claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(Helpers.ClaimType_UserEmail, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserTypeId.ToString()),
                    new Claim(Helpers.ClaimType_UserType, user.UserType.ToString())
                };
            }
            else
            {
                claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(Helpers.ClaimType_UserEmail, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserTypeId.ToString()),
                    new Claim(Helpers.ClaimType_UserType, user.UserType.ToString()),
                };
            }

            var roles = _userManager.GetRolesAsync(user).Result;

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GetUserEmailVerificationLink(string userToken)
        {
            var originUrls = new StringValues();
            //CHECK LATER TO SEE IF ANY ORIGIN HEADER WILL BE SENT WITH THE REQUEST IF THE FRONTEND AND BACKEND ARE IN THE SAME DOMAIN...THAT IS IF THERE IS NO CORS
            var originHeadersGotten = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Origin", out originUrls);
            if (originHeadersGotten)
            {
                var originUrl = originUrls.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(originUrl))
                {
                    return null;
                }

                var convertedUserToken = Uri.EscapeDataString(userToken);
                convertedUserToken = convertedUserToken.Replace('%', '-');
                string emailVerificationLink = originUrl + "/" + _configuration.GetValue<string>("UserEmailConfirmationLink") + "/" + convertedUserToken;
                return emailVerificationLink;
            }

            return null;
        }

        public async Task<ToRespond> LoginUser(UserForLogin userLoginDetails, string secretKey)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDetails.EmailAddress);

            if (user == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginDetails.Password, false);
            if (result.Succeeded)
            {
                object userInfo = null;
                if (user.UserType == Helpers.SiwesAdmin)
                {
                    userInfo = await _globalRepository.Get<SiwesAdmin>(user.UserTypeId);
                }
                else if (user.UserType == Helpers.SiwesCoordinator)
                {
                    var SiwesCoordinatorDetail = await _dataContext.SiwesCoordinators
                        .Where(s => s.SiwesCoordinatorId == user.UserTypeId).FirstOrDefaultAsync();
                    userInfo = SiwesCoordinatorDetail;
                }
                else if (user.UserType == Helpers.IndustrialSupervisor)
                {
                    var IndustrialSupervisorDetail = await _dataContext.IndustrialSupervisors
                        .Where(s => s.IndustrialSupervisorId == user.UserTypeId).FirstOrDefaultAsync();
                    userInfo = IndustrialSupervisorDetail;
                }
                else if (user.UserType == Helpers.Lecturer)
                {
                    var LecturerDetail = await _dataContext.Lecturers
                        .Where(s => s.LecturerId == user.UserTypeId).FirstOrDefaultAsync();

                    userInfo = LecturerDetail;
                }
                else if (user.UserType == Helpers.Student)
                {
                    var studentDetail = await _dataContext.Students
                        .Where(s => s.StudentId == user.UserTypeId)
                        .FirstOrDefaultAsync();
                    var student = _mapper.Map<StudentResponse>(studentDetail);
                    userInfo = student;
                }
                else
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.BadRequest,
                        StatusMessage = Helpers.StatusMessageBadRequest
                    };
                }

                user.SecondToLastLoginDateTime = user.LastLoginDateTime;
                user.LastLoginDateTime = DateTimeOffset.Now;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotSucceeded,
                        StatusMessage = "Not succeeded"
                    };
                }

                return new ToRespond()
                {
                    StatusCode = Helpers.Success,
                    ObjectValue = new UserDetails
                    {
                        Token = await GenerateJwtToken(user, secretKey),
                        User = user,
                        userProfile = userInfo
                    }
                };
            }

            else if (result.IsLockedOut)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.LockedOut,
                    StatusMessage = Helpers.StatusMessageLockedOut
                };
            }

            return new ToRespond()
            {
                StatusCode = Helpers.SignInError
            };
        }

    }
}
