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
    //[Authorize(Roles = "SiwesAdmin")]
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

        /// <summary>
        /// Comment On Student Log Book
        /// </summary>
        /// POST: api/lecturer/comment/1
        [HttpPost]
        [Route("comment/{logBookId}")]
        public async Task<ActionResult> LogBookComment(int logBookId, [FromBody] CommentRequest commentRequest)
        {
            var result = await _lecturerRepository.LogBookComment(logBookId,commentRequest);

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
        /// Comment On Student Log Book
        /// </summary>
        /// POST: api/lecturer/comment/1
        [HttpPost]
        [Route("grade/{logBookId}")]
        public async Task<ActionResult> LogBookGrade(int logBookId, [FromBody] GradeRequest gradeRequest)
        {
            var result = await _lecturerRepository.LogBookGrade(logBookId, gradeRequest);

            if (result.StatusCode == Helpers.Success)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
    }
}
