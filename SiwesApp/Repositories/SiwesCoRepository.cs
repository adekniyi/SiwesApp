using AutoMapper;
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
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using SiwesApp.Dtos.StudentDto;
using SiwesApp.Dtos.LecturerDto;

namespace SiwesApp.Repositories
{
    public class SiwesCoRepository : ISiwesCoordinatorRepo
    {
        private readonly ApplicationDataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IGlobalRepository _globalRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationRepo _authenticationRepository;
        private readonly ICloudinaryRepository _cloudinaryRepository;


        public SiwesCoRepository(ApplicationDataContext dataContext, UserManager<User> userManager,
            IGlobalRepository globalRepository, IAuthenticationRepo authenticationRepository,
            IMapper mapper, ICloudinaryRepository cloudinaryRepository)
        {
            _dataContext = dataContext;
            _userManager = userManager;
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
        public async Task<ToRespond> GetAllSiwesCos()
        {
            var siwesCo = await _dataContext.SiwesCoordinators.ToListAsync();

            if (siwesCo == null)
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
                ObjectValue = _mapper.Map<List<SiwesCoordinatorResponse>>(siwesCo)
            };
        }
        public async Task<ToRespond> GetOneSiwesCo(int siwesCoId)
        {
            var siwesCo = await _dataContext.SiwesCoordinators.FindAsync(siwesCoId);

            if (siwesCo == null)
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
                ObjectValue = _mapper.Map<SiwesCoordinatorResponse>(siwesCo)
            };
        }
        public async Task<ToRespond> MakePlacementEligible(int studentId)
        {

            var student = await _dataContext.Students.Where(x=>x.StudentId == studentId)
                                                      .Where(x => x.EligiblityStatus == Helpers.Pending)
                                                      .Include(x => x.Placement)
                                                      .FirstOrDefaultAsync();
            if (student == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            student.EligiblityStatus = Helpers.Eligible;
            _dataContext.Entry(student).State = EntityState.Modified;
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
                // await dbTransaction.CommitAsync();
                return new ToRespond()
                {
                    StatusCode = Helpers.Success,
                    ObjectValue = _mapper.Map<StudentResponse>(student),
                };

            }

            return new ToRespond()
            {
                StatusCode = Helpers.SaveError,
                StatusMessage = Helpers.StatusMessageSaveError
            };
        }
        public async Task<ToRespond> RejectPlacement(int studentId)
        {
            var student = await _dataContext.Students.Where(x => x.StudentId == studentId)
                                                     .Where(x => x.EligiblityStatus == Helpers.Pending)
                                                     .Include(x => x.Placement)
                                                     .FirstOrDefaultAsync();
            if (student == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            student.EligiblityStatus = Helpers.Rejected;
            _dataContext.Entry(student).State = EntityState.Modified;
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
                // await dbTransaction.CommitAsync();
                return new ToRespond()
                {
                    StatusCode = Helpers.Success,
                    ObjectValue = _mapper.Map<StudentResponse>(student),
                };

            }

            return new ToRespond()
            {
                StatusCode = Helpers.SaveError,
                StatusMessage = Helpers.StatusMessageSaveError
            };
        }
        public async Task<ToRespond> AssignStudentToLecturer(AssignStudentToLecturerRequest assignStudentToLecturer)
        {
            if(assignStudentToLecturer == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }

            List<Student> allStudents = new List<Student>();
            var assignStudentsToLecturer = new AssignStudentToLecturer();

            foreach (var studentId in assignStudentToLecturer.Students)
            {
                var student = _dataContext.Students.Where(x => x.StudentId == studentId).FirstOrDefault();

                if (student == null)
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.NotFound,
                        StatusMessage = Helpers.StatusMessageNotFound
                    };
                }

                allStudents.Add(student);
                //assignStudentsToLecturer.StudentId = studentId;
            }

            assignStudentsToLecturer.Student = allStudents;
            var lecturer = _dataContext.Lecturers.Where(x => x.LecturerId == assignStudentToLecturer.Lecturer).FirstOrDefault();
            //var IndustrialSupervisor = _dataContext.IndustrialSupervisors.Where(x => x.IndustrialSupervisorId == assignStudentToLecturer.IndustrialSupervisorId).FirstOrDefault();

            if (lecturer == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.NotFound,
                    StatusMessage = Helpers.StatusMessageNotFound
                };
            }
            assignStudentsToLecturer.LecturerId = assignStudentToLecturer.Lecturer;
            assignStudentsToLecturer.Lecturer = lecturer;
            assignStudentsToLecturer.IndustrialSupervisorId = assignStudentToLecturer.IndustrialSupervisorId;


            //var assignStudentsToLecturer = _mapper.Map<AssignStudentToLecturer>(assignStudentToLecturer);
            //assignStudentsToLecturer.Student = allStudents;
            foreach(var stu in assignStudentsToLecturer.Student)
            {
                assignStudentsToLecturer.StudentId = stu.StudentId;

                _globalRepository.Add(assignStudentsToLecturer);
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
                        ObjectValue = _mapper.Map<AssignStudentToLecturerResponse>(assignStudentsToLecturer),
                        StatusMessage = "Students Assigned To Lecturer Successfully"
                    };

                }
            }

            return new ToRespond()
            {
                StatusCode = Helpers.SaveError,
                StatusMessage = Helpers.StatusMessageSaveError
            };
        }
        public async Task<ToRespond> GetStudentLogBooks(int studentId)
        {
            var student = _dataContext.Students.Where(x => x.StudentId == studentId)
                                                .Include(x => x.LogBook)
                                                .FirstOrDefault();

            var assignedStudent = _dataContext.AssignStudentToLecturers.Where(x => x.StudentId == studentId)
                                                                       .SingleOrDefault();

            var lecturer = _dataContext.Lecturers.Find(assignedStudent.LecturerId);

            if (student == null)
            {
                return new ToRespond
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            var studentlogbook = _mapper.Map<StudentLogBook>(student);
            var lecturerRes = _mapper.Map<LecturerResponse>(lecturer);
            studentlogbook.Lecturer = lecturerRes;

            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                StatusMessage = Helpers.StatusMessageSuccess,
                ObjectValue = studentlogbook
            };
        }
        public async Task<ToRespond> GetStudentLogBook(int studentId, int logBookId)
        {
            var studentLogBook = await _dataContext.Students.Where(x => x.StudentId == studentId)
                                                    .Where(x=>x.LogBookId == logBookId)
                                                    .FirstOrDefaultAsync();

            var assignedStudent = _dataContext.AssignStudentToLecturers.Where(x => x.StudentId == studentId)
                                                                       .SingleOrDefault();

            var lecturer = _dataContext.Lecturers.Find(assignedStudent.LecturerId);

            if (studentLogBook == null)
            {
                return new ToRespond
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            var studentlogbook = _mapper.Map<StudentLogBook>(studentLogBook);
            var lecturerRes = _mapper.Map<LecturerResponse>(lecturer);
            studentlogbook.Lecturer = lecturerRes;

            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                StatusMessage = Helpers.StatusMessageSuccess,
                ObjectValue = studentlogbook
            };
        }

    }
}
