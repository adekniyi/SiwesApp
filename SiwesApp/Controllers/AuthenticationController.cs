using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiwesApp.Dtos.All;
using SiwesApp.Dtos.IndustrialSupervisorDto;
using SiwesApp.Dtos.LecturerDto;
using SiwesApp.Dtos.SiwesAdmin;
using SiwesApp.Dtos.SiwesCoOrdinatotDto;
using SiwesApp.Dtos.StudentDto;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepo _authenticationRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AuthenticationController(IMapper mapper, IConfiguration configuration, IAuthenticationRepo authenticationRepo)
        {
            _mapper = mapper;
            _configuration = configuration;
            _authenticationRepo = authenticationRepo;
        }

        /// <summary>
        /// LOGIN THE USER TO THE SYSTEM
        /// </summary>
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> PostLogin([FromBody] UserForLogin userForLogin)
        {
            var result = await _authenticationRepo.LoginUser(userForLogin, _configuration.GetValue<string>("AppSettings:Secret"));

            if (result.StatusCode == Helpers.Success)
            {
                result.StatusMessage = "Login Success!!!";
                var userDetails = (UserDetails)result.ObjectValue;
                var userInfoToReturn = _mapper.Map<UserLoginResponse>(userDetails);
                if (userDetails.User.UserType == Helpers.SiwesAdmin)
                {
                    //SUPERADMIN
                    userInfoToReturn.UserProfileInformation = _mapper.Map<SiwesAdminResponse>((SiwesAdmin)userDetails.userProfile);
                }
                else if (userDetails.User.UserType == Helpers.SiwesCoordinator)
                {
                    //ADMIN
                    userInfoToReturn.UserProfileInformation = _mapper.Map<SiwesCoordinatorResponse>((SiwesCoordinator)userDetails.userProfile);
                }
                else if (userDetails.User.UserType == Helpers.IndustrialSupervisor)
                {
                    //ADMIN
                    userInfoToReturn.UserProfileInformation = _mapper.Map<IndustrialSupervisorResponse>((IndustrialSupervisor)userDetails.userProfile);
                }
                else if (userDetails.User.UserType == Helpers.Lecturer)
                {
                    //STAFF
                    userInfoToReturn.UserProfileInformation = _mapper.Map<LecturerResponse>((Lecturer)userDetails.userProfile);
                }
                else
                {
                    //STUDENT
                    userInfoToReturn.UserProfileInformation = (StudentResponse)userDetails.userProfile;
                }
                result.ObjectValue = userInfoToReturn;
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
    }
}
