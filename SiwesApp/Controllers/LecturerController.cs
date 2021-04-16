using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiwesApp.Dtos.LecturerDto;
using SiwesApp.Interfaces;
using SiwesApp.Utils;
using System.Threading.Tasks;

namespace SiwesApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerRepo _lecturerRepository;

        public LecturerController(ILecturerRepo lecturerRepository)
        {
            _lecturerRepository = lecturerRepository;
        }

        /// <summary>
        /// CREATE A Lecturer FOR Siwes Application
        /// </summary>
        /// POST: api/lecturer
        [HttpPost]
        [Authorize(Roles ="SiwesAdmin")]
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

    }
}
