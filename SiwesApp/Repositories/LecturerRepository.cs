using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiwesApp.Data;
using SiwesApp.Dtos.All;
using SiwesApp.Dtos.LecturerDto;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using SiwesApp.Dtos.CommentAndGrade;

namespace SiwesApp.Repositories
{
    public class LecturerRepository : ILecturerRepo
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


        public LecturerRepository(ApplicationDataContext dataContext, UserManager<User> userManager,
            SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment, IGlobalRepository globalRepository, IAuthenticationRepo authenticationRepository,
            IMapper mapper,ICloudinaryRepository cloudinaryRepository)
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
        public async Task<ToRespond> CreateLecturer(LecturerRequest lecturerRequest)
        {
            if (lecturerRequest == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            if (await LecturerExists(lecturerRequest.EmailAddress))
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectExists,
                    StatusMessage = Helpers.StatusMessageObjectExists
                };
            }

            var lecturer = new Lecturer
            {
                EmailAddress = lecturerRequest.EmailAddress,
                FirstName = lecturerRequest.FirstName,
                LastName = lecturerRequest.LastName,
                PhoneNumber = lecturerRequest.PhoneNumber,
                Department = lecturerRequest.Department,
            };

            if (lecturerRequest.PictureUrl != null)
            {
                var result = _cloudinaryRepository.UploadFileToCloudinary(lecturerRequest.PictureUrl);
                var image = (RawUploadResult)result.ObjectValue;
                lecturer.PictureUrl = image.Uri.ToString();
            }

            var dbTransaction = await _dataContext.Database.BeginTransactionAsync();
            _globalRepository.Add(lecturer);
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
                    UserName = lecturerRequest.EmailAddress,
                    Email = lecturerRequest.EmailAddress,
                    UserTypeId = lecturer.LecturerId,
                    UserType = Helpers.Student
                };

                var password = (new Generate()).RandomPassword();

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    //ASSIGN STUDENT ROLE TO USER (Student)
                    string LecturerTypeRole = Helpers.LecturerRole;


                    var assignmentResult = await _userManager.AddToRoleAsync(user, LecturerTypeRole);
                    if (assignmentResult.Succeeded)
                    {
                        //THEN UPDATE Student TABLE USERID COLUMN WITH NEWLY CREATED USER ID
                        lecturer.UserId = user.Id;
                        var LecturerUpdateResult = _globalRepository.Update(lecturer);
                        if (!LecturerUpdateResult)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.NotSucceeded,
                                StatusMessage = "Not Suceeded"
                            };
                        }

                        var LecturerUpdateSaveResult = await _globalRepository.SaveAll();
                        if (!LecturerUpdateSaveResult.HasValue)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.SaveError,
                                StatusMessage = Helpers.StatusMessageSaveError
                            };
                        }

                        if (!LecturerUpdateSaveResult.Value)
                        {
                            return new ToRespond()
                            {
                                StatusCode = Helpers.SaveNoRowAffected,
                                StatusMessage = "Can not save Row affected"
                            };
                        }

                        var userTokenVal = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        string hashedEmail = GetHashedEmail(user.Email);
             
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
                                ObjectValue = _mapper.Map<LecturerResponse>(lecturer),
                                StatusMessage = "Lecturer Created Successfully!!!"
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

        public async Task<bool> LecturerExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email.ToUpper() == email.ToUpper());
        }

        public async Task<ToRespond> GetAllLecturers()
        {
            var lecturers = await _dataContext.Lecturers.ToListAsync();

            if (lecturers == null)
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
                ObjectValue = _mapper.Map<List<LecturerResponse>>(lecturers)
            };
        }

        public async Task<ToRespond> GetOneLecturer(int lecturerId)
        {
            var lecturer = await _dataContext.Lecturers.FindAsync(lecturerId);

            if (lecturer == null)
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
                ObjectValue = _mapper.Map<LecturerResponse>(lecturer)
            };
        }

        public async Task<ToRespond> CommentOnLogBook(int logBookId, CommentRequest commentRequest)
        {
            var logBook = await _dataContext.LogBooks.FindAsync(logBookId);
            if(logBook==null|| commentRequest == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            var userId = Int32.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            var industrialSupervisor = await _dataContext.IndustrialSupervisors.Where(a => a.UserId == userId).SingleOrDefaultAsync();
            var lecturer = await _dataContext.Lecturers.Where(a => a.UserId == userId).SingleOrDefaultAsync();

            var comment = new Comment();
            if (lecturer != null)
            {
                comment.CommentterId = lecturer.LecturerId;
                comment.CommentDetail = commentRequest.CommentDetail;
                comment.Lecturer = lecturer;
                comment.LogBook = logBook;
                comment.LogBookId = logBook.LogBookId;

                _globalRepository.Add(comment);

                logBook.Comment.Add(comment);

                _dataContext.Entry(logBook).State = EntityState.Modified;

            }
            else if(industrialSupervisor != null)
            {
                comment.CommentterId = industrialSupervisor.IndustrialSupervisorId;
                comment.CommentDetail = commentRequest.CommentDetail;
                comment.IndustrialSupervisor = industrialSupervisor;
                comment.LogBook = logBook;
                comment.LogBookId = logBook.LogBookId;
                logBook.Comment.Add(comment);

                _dataContext.Entry(logBook).State = EntityState.Modified;

                _globalRepository.Add(comment);
            }
            else
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            var result = await _globalRepository.SaveAll();

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
                    ObjectValue = _mapper.Map<CommentResponse>(comment),
                    StatusMessage = "Your Comment Was Successfully!!!"
                };

            }

            return new ToRespond()
            {
                StatusCode = Helpers.SaveError,
                StatusMessage = Helpers.StatusMessageSaveError
            };

        }

        public async Task<ToRespond> GradeLogBook(int logBookId, GradeRequest gradeRequest)
        {
            var logBook = await _dataContext.LogBooks.FindAsync(logBookId);
            if (logBook == null || gradeRequest == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            var userId = Int32.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            var industrialSupervisor = await _dataContext.IndustrialSupervisors.Where(a => a.UserId == userId).SingleOrDefaultAsync();
            var lecturer = await _dataContext.Lecturers.Where(a => a.UserId == userId).SingleOrDefaultAsync();

            var grade = new Grade();
            if (lecturer != null)
            {
                grade.GraderId = lecturer.LecturerId;
                grade.ObtainedGrade = gradeRequest.ObtainedGrade;
                grade.ObtainableGrade = gradeRequest.ObtainableGrade;
                grade.Lecturer = lecturer;
                grade.LogBook = logBook;
                grade.LogBookId = logBook.LogBookId;

                _globalRepository.Add(grade);

                logBook.Grade.Add(grade);

                _dataContext.Entry(logBook).State = EntityState.Modified;

            }
            else if (industrialSupervisor != null)
            {
                grade.GraderId = industrialSupervisor.IndustrialSupervisorId;
                grade.ObtainedGrade = gradeRequest.ObtainedGrade;
                grade.ObtainableGrade = gradeRequest.ObtainableGrade;
                grade.IndustrialSupervisor = industrialSupervisor;
                grade.LogBook = logBook;
                grade.LogBookId = logBook.LogBookId;

                _globalRepository.Add(grade);

                logBook.Grade.Add(grade);

                _dataContext.Entry(logBook).State = EntityState.Modified;

            }
            else
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            var result = await _globalRepository.SaveAll();

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
                    ObjectValue = _mapper.Map<GradeResponse>(grade),
                    StatusMessage = "Student Log Book Graded Successfully!!!"
                };

            }

            return new ToRespond()
            {
                StatusCode = Helpers.SaveError,
                StatusMessage = Helpers.StatusMessageSaveError
            };
        }

        public async Task<ToRespond> GetACommenttedLogBook(int commentId)
        {
            var comment =  await _dataContext.Comments.Where(x=>x.CommentId ==commentId)
                                                .Include(x=>x.LogBook)
                                                .Include(x=>x.Lecturer)
                                                .Include(x=>x.IndustrialSupervisor)
                                                .FirstOrDefaultAsync();
            if(comment == null)
            {
                return new ToRespond
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                ObjectValue = _mapper.Map<CommentResponse>(comment)
            };

        }

        public async Task<ToRespond> GetAGradedLogBook(int gradeId)
        {
            var grade = await _dataContext.Grades.Where(x => x.GradeId == gradeId)
                                                .Include(x => x.LogBook)
                                                .Include(x => x.Lecturer)
                                                .Include(x => x.IndustrialSupervisor)
                                                .FirstOrDefaultAsync();
            if (grade == null)
            {
                return new ToRespond
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                ObjectValue = _mapper.Map<GradeResponse>(grade),
            };
        }

        public async Task<ToRespond> GetAllStudentCommenttedLogBook()
        {
            var comment = await _dataContext.Comments
                                               .Include(x => x.LogBook)
                                               .Include(x => x.Lecturer)
                                               .Include(x => x.IndustrialSupervisor)
                                               .ToListAsync();
            if (!comment.Any())
            {
                return new ToRespond
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                ObjectValue = _mapper.Map<List<CommentResponse>>(comment)
            };
        }

        public async Task<ToRespond> GetAllStudentGradedLogBook()
        {
            var grade = await _dataContext.Grades
                                               .Include(x => x.LogBook)
                                               .Include(x => x.Lecturer)
                                               .Include(x => x.IndustrialSupervisor)
                                               .ToListAsync();
            if (!grade.Any())
            {
                return new ToRespond
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                ObjectValue = _mapper.Map<List<GradeResponse>>(grade),
            };
        }
    }
}
