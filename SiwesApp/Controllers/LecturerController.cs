using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiwesApp.Dtos.CommentAndGrade;
using SiwesApp.Dtos.LecturerDto;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILecturerRepo _lecturerRepository;

        public LecturerController(UserManager<User> userManager, ILecturerRepo lecturerRepository)
        {
            _userManager = userManager;
            _lecturerRepository = lecturerRepository;
        }

        /// <summary>
        /// CREATE A Lecturer FOR Siwes Application
        /// </summary>
        /// POST: api/lecturer
        [HttpPost]
        public async Task<ActionResult> CreateLecturer([FromForm] LecturerRequest lecturerRequest)
        {
            var result = await _lecturerRepository.CreateLecturer(lecturerRequest);

            if (result.StatusCode == Helpers.Success)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }

        /// <summary>
        /// Get All Lecturers FOR AN Siwes Application
        /// </summary>
        /// Get: api/lecturer
        [HttpGet]
        public async Task<ActionResult> GetAllLecturers()
        {
            var result = await _lecturerRepository.GetAllLecturers();

            if (result.StatusCode == Helpers.Success)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }

        /// <summary>
        /// Get One Lecturer FOR AN Siwes Application
        /// </summary>
        /// Get: api/lecturer/1
        [HttpGet]
        [Route("{lecturerId}")]
        public async Task<ActionResult> GetOneLecturer(int lecturerId)
        {
            var result = await _lecturerRepository.GetOneLecturer(lecturerId);

            if (result.StatusCode == Helpers.Success)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }

        ///// <summary>
        ///// Comment On Student Log Book
        ///// </summary>
        ///// POST: api/lecturer/1/comment
        //[HttpPost]
        //[Route("{logBookId}/comment")]
        //public async Task<ActionResult> CommentOnLogBook(int logBookId, [FromBody] CommentRequest commentRequest)
        //{
        //    var result = await _lecturerRepository.CommentOnLogBook(logBookId,commentRequest);

        //    if (result.StatusCode == Helpers.Success)
        //    {
        //        return StatusCode(StatusCodes.Status200OK, result);
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, result);
        //    }
        //}

        ///// <summary>
        ///// Comment On Student Log Book
        ///// </summary>
        ///// POST: api/lecturer/1/grade
        //[HttpPost]
        //[Route("{logBookId}/grade")]
        //public async Task<ActionResult> GradeLogBook(int logBookId, [FromBody] GradeRequest gradeRequest)
        //{
        //    var result = await _lecturerRepository.GradeLogBook(logBookId, gradeRequest);

        //    if (result.StatusCode == Helpers.Success)
        //    {
        //        return StatusCode(StatusCodes.Status200OK, result);
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, result);
        //    }
        //}

        ///// <summary>
        ///// Get One Log Book Comment
        ///// </summary>
        ///// Get: api/lecturer/1/comment/1
        //[HttpGet]
        //[Route("{logBookId}/comment/{commentId}")]
        //public async Task<ActionResult> GetACommenttedLogBook(int commentId)
        //{
        //    var result = await _lecturerRepository.GetACommenttedLogBook(commentId);

        //    if (result.StatusCode == Helpers.Success)
        //    {
        //        return StatusCode(StatusCodes.Status200OK, result);
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, result);
        //    }
        //}

        ///// <summary>
        ///// Get One Log Book Grade
        ///// </summary>
        ///// Get: api/lecturer/1/grade/1
        //[HttpGet]
        //[Route("{logBookId}/grade/{commentId}")]
        //public async Task<ActionResult> GetAGradedLogBook(int gradeId)
        //{
        //    var result = await _lecturerRepository.GetAGradedLogBook(gradeId);

        //    if (result.StatusCode == Helpers.Success)
        //    {
        //        return StatusCode(StatusCodes.Status200OK, result);
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, result);
        //    }
        //}

        ///// <summary>
        ///// Get All Log Book Comment
        ///// </summary>
        ///// Get: api/lecturer/1/comment
        //[HttpGet]
        //[Route("{logBookId}/comment")]
        //public async Task<ActionResult> GetAllStudentCommenttedLogBook()
        //{
        //    var result = await _lecturerRepository.GetAllStudentCommenttedLogBook();

        //    if (result.StatusCode == Helpers.Success)
        //    {
        //        return StatusCode(StatusCodes.Status200OK, result);
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, result);
        //    }
        //}

        ///// <summary>
        ///// Get All Log Book Grade
        ///// </summary>
        ///// Get: api/lecturer/1/grade
        //[HttpGet]
        //[Route("{logBookId}/grade")]
        //public async Task<ActionResult> GetAllStudentGradedLogBook()
        //{
        //    var result = await _lecturerRepository.GetAllStudentGradedLogBook();

        //    if (result.StatusCode == Helpers.Success)
        //    {
        //        return StatusCode(StatusCodes.Status200OK, result);
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, result);
        //    }
        //}
    }
}
