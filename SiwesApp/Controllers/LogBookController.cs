using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiwesApp.Dtos.CommentAndGrade;
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
    public class LogBookController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogBookRepository _logBookRepository;
        public LogBookController(UserManager<User> userManager, ILogBookRepository logBookRepository)
        {
            _userManager = userManager;
            _logBookRepository = logBookRepository;
        }

        /// <summary>
        /// Fill A Student Log Book
        /// </summary>
        /// POST: api/logbook
        [HttpPost]
        public async Task<ActionResult> FillLogBook([FromBody] LogBookRequest logBookRequest)
        {
            var result = await _logBookRepository.FillLogBook(logBookRequest);

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
        /// POST: api/logbook/1/comment
        [HttpPost]
        [Route("{logBookId}/comment")]
        public async Task<ActionResult> CommentOnLogBook(int logBookId, [FromBody] CommentRequest commentRequest)
        {
            var result = await _logBookRepository.CommentOnLogBook(logBookId, commentRequest);

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
        /// POST: api/logbook/1/grade
        [HttpPost]
        [Route("{logBookId}/grade")]
        public async Task<ActionResult> GradeLogBook(int logBookId, [FromBody] GradeRequest gradeRequest)
        {
            var result = await _logBookRepository.GradeLogBook(logBookId, gradeRequest);

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
        /// Get One Log Book Comment
        /// </summary>
        /// Get: api/logbook/1/comment/1
        [HttpGet]
        [Route("{logBookId}/comment/{commentId}")]
        public async Task<ActionResult> GetACommenttedLogBook(int commentId)
        {
            var result = await _logBookRepository.GetACommenttedLogBook(commentId);

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
        /// Get One Log Book Grade
        /// </summary>
        /// Get: api/logbook/1/grade/1
        [HttpGet]
        [Route("{logBookId}/grade/{commentId}")]
        public async Task<ActionResult> GetAGradedLogBook(int gradeId)
        {
            var result = await _logBookRepository.GetAGradedLogBook(gradeId);

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
        /// Get All Log Book Comment
        /// </summary>
        /// Get: api/logbook/1/comment
        [HttpGet]
        [Route("{logBookId}/comment")]
        public async Task<ActionResult> GetAllStudentCommenttedLogBook()
        {
            var result = await _logBookRepository.GetAllStudentCommenttedLogBook();

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
        /// Get All Log Book Grade
        /// </summary>
        /// Get: api/logbook/1/grade
        [HttpGet]
        [Route("{logBookId}/grade")]
        public async Task<ActionResult> GetAllStudentGradedLogBook()
        {
            var result = await _logBookRepository.GetAllStudentGradedLogBook();

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
