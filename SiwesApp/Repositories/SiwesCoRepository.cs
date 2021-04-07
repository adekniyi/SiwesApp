using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiwesApp.Data;
using SiwesApp.Dtos.All;
using SiwesApp.Dtos.SiwesCoOrdinatotDto;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace SiwesApp.Repositories
{
    public class SiwesCoRepository : ISiwesCoordinatorRepo
    {
        private readonly ApplicationDataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGlobalRepository _globalRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationRepo _authenticationRepository;
        private readonly ICloudinaryRepository _cloudinaryRepository;


        public SiwesCoRepository(ApplicationDataContext dataContext, UserManager<User> userManager,
            SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment, IGlobalRepository globalRepository, IAuthenticationRepo authenticationRepository,
            IMapper mapper, ICloudinaryRepository cloudinaryRepository)
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
            _cloudinaryRepository = cloudinaryRepository;
        }
        public async Task<ToRespond> CreateSiwesCo(SiwesCoordinatorRequest siwesCoordinatorRequest)
        {
            if (siwesCoordinatorRequest == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            if (await SiwesCoordinatorExists(siwesCoordinatorRequest.EmailAddress))
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectExists,
                    StatusMessage = Helpers.StatusMessageObjectExists
                };
            }

            var siwesCoordinator = new SiwesCoordinator
            {
                EmailAddress = siwesCoordinatorRequest.EmailAddress,
                FirstName = siwesCoordinatorRequest.FirstName,
                LastName = siwesCoordinatorRequest.LastName,
                PhoneNumber = siwesCoordinatorRequest.PhoneNumber,
                Department = siwesCoordinatorRequest.Department,
            };

            if (siwesCoordinatorRequest.PictureUrl != null)
            {
                var result = _cloudinaryRepository.UploadFileToCloudinary(siwesCoordinatorRequest.PictureUrl);
                var image = (RawUploadResult)result.ObjectValue;
                siwesCoordinator.PictureUrl = image.Uri.ToString();
            }

            var dbTransaction = await _dataContext.Database.BeginTransactionAsync();
            _globalRepository.Add(siwesCoordinator);
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
                    UserName = siwesCoordinatorRequest.EmailAddress,
                    Email = siwesCoordinatorRequest.EmailAddress,
                    UserTypeId = siwesCoordinator.SiwesCoordinatorId,
                    UserType = Helpers.SiwesCoordinator
                };

                var password = (new Generate()).RandomPassword();

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    //ASSIGN STUDENT ROLE TO USER (Student)
                    string SiwesCoordinatorTypeRole = Helpers.SiwesCoordinatorRole;


                    var assignmentResult = await _userManager.AddToRoleAsync(user, SiwesCoordinatorTypeRole);
                    if (assignmentResult.Succeeded)
                    {
                        //THEN UPDATE Student TABLE USERID COLUMN WITH NEWLY CREATED USER ID
                        siwesCoordinator.UserId = user.Id;
                        var SiwesCoordinatorUpdateResult = _globalRepository.Update(siwesCoordinator);
                        if (!SiwesCoordinatorUpdateResult)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.NotSucceeded,
                                StatusMessage = "Not Suceeded"
                            };
                        }

                        var SiwesCoordinatorUpdateSaveResult = await _globalRepository.SaveAll();
                        if (!SiwesCoordinatorUpdateSaveResult.HasValue)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.SaveError,
                                StatusMessage = Helpers.StatusMessageSaveError
                            };
                        }

                        if (!SiwesCoordinatorUpdateSaveResult.Value)
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
                                ObjectValue = _mapper.Map<SiwesCoordinatorResponse>(siwesCoordinator),
                                StatusMessage = "Siwes Coordinator Created Successfully!!!"
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

        public async Task<bool> SiwesCoordinatorExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email.ToUpper() == email.ToUpper());
        }
    }
}
