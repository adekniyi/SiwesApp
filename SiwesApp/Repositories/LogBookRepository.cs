using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SiwesApp.Data;
using SiwesApp.Dtos.All;
using SiwesApp.Dtos.CommentAndGrade;
using SiwesApp.Dtos.StudentDto;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Repositories
{
    public class LogBookRepository : ILogBookRepository
    {
        private readonly ApplicationDataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGlobalRepository _globalRepository;
        private readonly IMapper _mapper;

        public LogBookRepository(ApplicationDataContext dataContext, IHttpContextAccessor httpContextAccessor, 
            IGlobalRepository globalRepository, IMapper mapper)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _globalRepository = globalRepository;
            _mapper = mapper;
        }
      

        public async Task<ToRespond> FillLogBook(LogBookRequest logBookRequest)
        {
            if (logBookRequest == null)
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

            if (student.EligiblityStatus == Helpers.Eligible)
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

        public async Task<ToRespond> CommentOnLogBook(int logBookId, CommentRequest commentRequest)
        {
            var logBook = await _dataContext.LogBooks.FindAsync(logBookId);
            if (logBook == null || commentRequest == null)
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
            else if (industrialSupervisor != null)
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
            var comment = await _dataContext.Comments.Where(x => x.CommentId == commentId)
                                                .Include(x => x.LogBook)
                                                .Include(x => x.Lecturer)
                                                .Include(x => x.IndustrialSupervisor)
                                                .FirstOrDefaultAsync();
            if (comment == null)
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
