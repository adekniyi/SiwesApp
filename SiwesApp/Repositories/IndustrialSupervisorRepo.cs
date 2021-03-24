using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiwesApp.Data;
using SiwesApp.Dtos.All;
using SiwesApp.Dtos.IndustrialSupervisorDto;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Repositories
{
    public class IndustrialSupervisorRepo : IIndustrialSupervisorRepo
    {
        private readonly ApplicationDataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGlobalRepository _globalRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAuthenticationRepo _authenticationRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndustrialSupervisorRepo(ApplicationDataContext dataContext, UserManager<User> userManager,
            SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment, IGlobalRepository globalRepository, IAuthenticationRepo authenticationRepository,
            IMapper mapper)
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
        }
        public async Task<ToRespond> CreateIndustrialSupervisor(IndustrialSupervisorRequest industrialSupervisorRequest)
        {
            if (industrialSupervisorRequest == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            if (await IndustrialSupervisorExists(industrialSupervisorRequest.EmailAddress))
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectExists,
                    StatusMessage = Helpers.StatusMessageObjectExists
                };
            }

            var industrialSupervisor = new IndustrialSupervisor
            {
                EmailAddress = industrialSupervisorRequest.EmailAddress,
                FirstName = industrialSupervisorRequest.FirstName,
                LastName = industrialSupervisorRequest.LastName,
                PhoneNumber = industrialSupervisorRequest.PhoneNumber,
                CompanyAddress = industrialSupervisorRequest.CompanyAddress,
                CompanyName = industrialSupervisorRequest.CompanyName,
                SectionOfWork = industrialSupervisorRequest.SectionOfWork
            };

            string uniqueFileName = null;
            if (industrialSupervisorRequest.PictureUrl != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + industrialSupervisorRequest.PictureUrl.FileName;

                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                industrialSupervisorRequest.PictureUrl.CopyTo(new FileStream(filePath, FileMode.Create));
                industrialSupervisor.PictureUrl = uniqueFileName;
            }

            var dbTransaction = await _dataContext.Database.BeginTransactionAsync();
            _globalRepository.Add(industrialSupervisor);
            var saveVal = await _globalRepository.SaveAll();

            if (saveVal != null)
            {
                if (!saveVal.Value)
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.SaveNoRowAffected,
                        StatusMessage = "Can Not Save"
                    };
                }
                var user = new User
                {
                    UserName = industrialSupervisorRequest.EmailAddress,
                    Email = industrialSupervisorRequest.EmailAddress,
                    UserTypeId = industrialSupervisor.IndustrialSupervisorId,
                    UserType = Helpers.Student
                };

                var password = (new Generate()).RandomPassword();

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    //ASSIGN STUDENT ROLE TO USER (Student)
                    string IndustrialSupervisorTypeRole = Helpers.IndustrialSupervisorRole;


                    var assignmentResult = await _userManager.AddToRoleAsync(user, IndustrialSupervisorTypeRole);
                    if (assignmentResult.Succeeded)
                    {
                        //THEN UPDATE Student TABLE USERID COLUMN WITH NEWLY CREATED USER ID
                        industrialSupervisor.UserId = user.Id;
                        var IndustrialSupervisorUpdateResult = _globalRepository.Update(industrialSupervisor);
                        if (!IndustrialSupervisorUpdateResult)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.NotSucceeded,
                                StatusMessage = "Not Suceeded"
                            };
                        }

                        var IndustrialSupervisorUpdateSaveResult = await _globalRepository.SaveAll();
                        if (!IndustrialSupervisorUpdateSaveResult.HasValue)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.SaveError,
                                StatusMessage = Helpers.StatusMessageSaveError
                            };
                        }

                        if (!IndustrialSupervisorUpdateSaveResult.Value)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.SaveNoRowAffected,
                                StatusMessage = "Can not save Row affected"
                            };
                        }

                        //SEND MAIL TO Student TO CONFIRM EMAIL
                        var userTokenVal = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        string hashedEmail = GetHashedEmail(user.Email);
                        /*var shortToken = userTokenVal.Substring(0, 7);
                        user.ShortToken = shortToken;
                        user.LongToken = userTokenVal;
                        _ = _globalRepository.SaveAll();*/
                        var fullToken = userTokenVal + "#" + hashedEmail;

                        var emailVerificationLink = _authenticationRepository.GetUserEmailVerificationLink(fullToken);
                        if (emailVerificationLink == null)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.ObjectNull,
                                StatusMessage = "Email Verification Link Generation Error"
                            };
                        }
                        else
                        {
                            await dbTransaction.CommitAsync();
                            return new ToRespond()
                            {
                                StatusCode = Helpers.Success,
                                ObjectValue = _mapper.Map<IndustrialSupervisorResponse>(industrialSupervisor),
                                StatusMessage = "Student Created Successfully!!!"
                            };
                        }
                    }

                }

                return new ToRespond()
                {
                    StatusCode = Helpers.NotSucceeded,
                    StatusMessage = "Can not Suceed"
                };
            }

            return new ToRespond()
            {
                StatusCode = Helpers.SaveError,
                StatusMessage = Helpers.StatusMessageSaveError
            };
        }

        private string GetHashedEmail(string emailVal)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(emailVal));
        }

        public async Task<bool> IndustrialSupervisorExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email.ToUpper() == email.ToUpper());
        }
    }
}
