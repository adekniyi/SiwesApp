using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SiwesApp.Data;
using SiwesApp.Dtos.All;
using SiwesApp.Dtos.StudentDto;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using CloudinaryDotNet.Actions;

namespace SiwesApp.Repositories
{
    public class StudentRepository : IStudentRepo
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
        private readonly ICloudinaryRepository _cloudinaryRepository;


        public StudentRepository(ApplicationDataContext dataContext, UserManager<User> userManager, 
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
        public async Task<ToRespond> CreateStudent(StudentRequest studentRequest)
        {
            if (studentRequest == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            if (await StudentExists(studentRequest.EmailAddress))
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectExists,
                    StatusMessage = Helpers.StatusMessageObjectExists
                };
            }

            var student = new Student
            {
                EmailAddress = studentRequest.EmailAddress,
                FirstName = studentRequest.FirstName,
                LastName = studentRequest.LastName,
                PhoneNumber = studentRequest.PhoneNumber,
                Address = studentRequest.Address,
                Gender = studentRequest.Gender,
                DOB = studentRequest.DOB,
                State = studentRequest.State,
                LGA = studentRequest.LGA,
            };

            if (studentRequest.StudentPicture != null)
            {
                var result = _cloudinaryRepository.UploadFileToCloudinary(studentRequest.StudentPicture);
                var image = (RawUploadResult)result.ObjectValue;
                student.PictureUrl = image.Uri.ToString();
            }

            var dbTransaction = await _dataContext.Database.BeginTransactionAsync();
            _globalRepository.Add(student);
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
                    UserName = studentRequest.EmailAddress,
                    Email = studentRequest.EmailAddress,
                    UserTypeId = student.StudentId,
                    UserType = Helpers.Student
                };

                var password = (new Generate()).RandomPassword();

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    //ASSIGN STUDENT ROLE TO USER (Student)
                    string StudentTypeRole = Helpers.StudentRole;


                    var assignmentResult = await _userManager.AddToRoleAsync(user, StudentTypeRole);
                    if (assignmentResult.Succeeded)
                    {
                        //THEN UPDATE Student TABLE USERID COLUMN WITH NEWLY CREATED USER ID
                        student.UserId = user.Id;
                        var StudentUpdateResult = _globalRepository.Update(student);
                        if (!StudentUpdateResult)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.NotSucceeded,
                                StatusMessage = "Not Suceeded"
                            };
                        }

                        var StudentUpdateSaveResult = await _globalRepository.SaveAll();
                        if (!StudentUpdateSaveResult.HasValue)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.SaveError,
                                StatusMessage = Helpers.StatusMessageSaveError
                            };
                        }

                        if (!StudentUpdateSaveResult.Value)
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
                            ObjectValue = _mapper.Map<StudentResponse>(student),
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

        public async Task<bool> StudentExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email.ToUpper() == email.ToUpper());
        }

        public async Task<ToRespond> GetAllStudents()
        {
            var students = await _dataContext.Students.ToListAsync();

            if (students == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            };


            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                StatusMessage = Helpers.StatusMessageSuccess,
                ObjectValue = _mapper.Map<List<StudentResponse>>(students)
            };
        }

        public async Task<ToRespond> GetOneStudent(int studentId)
        {
            var student = await _dataContext.Students.FindAsync(studentId);

            if(student == null)
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
                StatusMessage = Helpers.StatusMessageSuccess,
                ObjectValue = _mapper.Map<StudentResponse>(student)
            };
        }

        public async Task<ToRespond> StudentPlacement(PlacementRequestDto placementRequest)
        {
            if(placementRequest == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            //var studentId = Int32.Parse(_httpContextAccessor.HttpContext.User.Claims
            //                                .FirstOrDefault(x => x.Type == Helpers.ClaimType_StudentId)
            //                                .Value);

            var userId = Int32.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            var student = await _dataContext.Students.Where(a => a.UserId == userId).SingleOrDefaultAsync();
            if(student == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };

            }    
            //var student = await _dataContext.Students.FindAsync(studentId);

            var placement = new Placement
            {
                MatricNumber = placementRequest.MatricNumber,
                FullName = placementRequest.FirstName + placementRequest.LastName,
                RegistrationNumber = placementRequest.RegistrationNumber,
                Department = placementRequest.Department,
                Programm = placementRequest.Programm,
                Level = placementRequest.Level,
                CompanyName = placementRequest.CompanyName,
                CompanyAddress = placementRequest.CompanyAddress,
                SectionOfWork = placementRequest.SectionOfWork,
                EmailAddressOfCompany = placementRequest.EmailAddressOfCompany,
            };

            if (placementRequest.OfferLetter != null || student.PictureUrl != null)
            {
                var resultImage = _cloudinaryRepository.UploadFileToCloudinary(placementRequest.OfferLetter);
                var image = (RawUploadResult)resultImage.ObjectValue;
                placement.OfferLetter = image.Uri.ToString();
                placement.StudentPicture = student.PictureUrl;
            }

            var dbTransaction = await _dataContext.Database.BeginTransactionAsync();
            _globalRepository.Add(placement);
            student.EligiblityStatus = Helpers.Pending;
            student.Placement = placement;
            var result = await _globalRepository.SaveAll();
            student.PlacementId = placement.PlacementId;
            _dataContext.Entry(student).State = EntityState.Modified;

             if (result != null)
            {
                if (!result.Value)
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.SaveError,
                        StatusMessage = Helpers.StatusMessageSaveError
                    };
                }
                await dbTransaction.CommitAsync();
                return new ToRespond()
                {
                    StatusCode = Helpers.Success,
                    ObjectValue = _mapper.Map<PlacementResponse>(placement),
                    StatusMessage = "Placement Registered Successfully!!!"
                };

            }

            return new ToRespond()
            {
                StatusCode = Helpers.SaveError,
                StatusMessage = Helpers.StatusMessageSaveError
            };
        }

        public async Task<ToRespond> GetAllPendingStudentsPlacement()
        {
            var students = await _dataContext.Students.Where(x => x.EligiblityStatus == Helpers.Pending)
                                                .Include(x => x.Placement)
                                                .ToListAsync();
            if (!students.Any())
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
                ObjectValue = _mapper.Map<List<StudentResponse>>(students),
            };
        }

        public async Task<ToRespond> GetAllEliigibleStudentsPlacement()
        {
            var students = await _dataContext.Students.Where(x => x.EligiblityStatus == Helpers.Eligible)
                                                .Include(x => x.Placement)
                                                .ToListAsync();
            if (students == null)
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
                ObjectValue = _mapper.Map<List<StudentResponse>>(students),
            };
        }

        public async Task<ToRespond> GetAllRejectedStudentsPlacement()
        {
            var students = await _dataContext.Students.Where(x => x.EligiblityStatus == Helpers.Rejected)
                                               .Include(x => x.Placement)
                                               .ToListAsync();
            if (students == null)
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
                ObjectValue = _mapper.Map<List<StudentResponse>>(students),
            };
        }

        public async Task<ToRespond> FillLogBook(LogBookRequest logBookRequest)
        {
            if(logBookRequest == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            var userId = Int32.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            var student = await _dataContext.Students.Where(a => a.UserId == userId).SingleOrDefaultAsync();
            if (student == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };

            }

            if(student.EligiblityStatus == Helpers.Eligible)
            {
                var logbook = _mapper.Map<LogBook>(logBookRequest);
                logbook.StudentId = student.StudentId;
                logbook.Student = student;
                //student.LogBook.Add(logbook);

                _globalRepository.Add(logbook);
                var result = await _globalRepository.SaveAll();


                foreach (var stuLogBook in student.LogBook)
                {
                    stuLogBook.LogBookId = logbook.LogBookId;
                    stuLogBook.Student = student;
                    stuLogBook.StudentId = student.StudentId;
                    stuLogBook.Time = logbook.Time;
                    stuLogBook.Description = logbook.Description;
                }

                _dataContext.Entry(student).State = EntityState.Modified;

                if (result != null)
                {
                    if (!result.Value)
                    {
                        return new ToRespond()
                        {
                            StatusCode = Helpers.SaveError,
                            StatusMessage = Helpers.StatusMessageSaveError
                        };
                    }
                    return new ToRespond()
                    {
                        StatusCode = Helpers.Success,
                        ObjectValue = _mapper.Map<LogBookResponse>(logbook),
                        StatusMessage = "Student LogBook created Successfully!!!"
                    };

                }
            }
            else
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.SaveError,
                    StatusMessage = "Non Eligible Student Cannot fill the form"
                };
            }

            return new ToRespond()
            {
                StatusCode = Helpers.SaveError,
                StatusMessage = Helpers.StatusMessageSaveError
            };
        }
    }
}
