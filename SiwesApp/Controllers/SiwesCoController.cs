using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiwesApp.Dtos.SiwesCoOrdinatotDto;
using SiwesApp.Interfaces;
using SiwesApp.Utils;
using System.Threading.Tasks;

namespace SiwesApp.Controllers
{
    [Authorize(Roles = "SiwesAdmin, SiwesCoordinator")]
    [Route("api/[controller]")]
    [ApiController]
    public class SiwesCoController : ControllerBase
    {
        private readonly ISiwesCoordinatorRepo _siwesCoRepository;

        public SiwesCoController(ISiwesCoordinatorRepo siwesCoRepository)
        {
            _siwesCoRepository = siwesCoRepository;
        }

        /// <summary>
        /// CREATE SiwesCoordinator FOR A Siwes Application
        /// </summary>
        /// POST: api/siwescoordinator
        [HttpPost]
        public async Task<ActionResult> CreateSiwesCoordinator([FromForm] SiwesCoordinatorRequest siwesCoordinatorRequest)
        {
            var result = await _siwesCoRepository.CreateSiwesCo(siwesCoordinatorRequest);

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
        /// Get All SiwesCos FOR AN Siwes Application
        /// </summary>
        /// Get: api/siwesCo
        [HttpGet]
        public async Task<ActionResult> GetAllSiwesCo()
        {
            var result = await _siwesCoRepository.GetAllSiwesCos();

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
        /// Get One siwesCo FOR AN Siwes Application
        /// </summary>
        /// Get: api/siwesCo/1
        [HttpGet]
        [Route("{siwesCoId}")]
        public async Task<ActionResult> GetOneSiwesCo(int siwesCoId)
        {
            var result = await _siwesCoRepository.GetOneSiwesCo(siwesCoId);

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
        /// Make Student Eligible For Placement
        /// </summary>
        /// Get: api/siwesCo/eligible/1
        [HttpGet]
        [Route("eligible/{studentId}")]
        public async Task<ActionResult> MakePlacementEligible(int studentId)
        {
            var result = await _siwesCoRepository.MakePlacementEligible(studentId);

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
        /// Reject Student Placement
        /// </summary>
        /// Get: api/siwesCo/reject/1
        [HttpGet]
        [Route("reject/{studentId}")]
        public async Task<ActionResult> RejectPlacement(int studentId)
        {
            var result = await _siwesCoRepository.RejectPlacement(studentId);

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
        /// Assign Students To Lecturer FOR A Siwes Application
        /// </summary>
        /// POST: api/siwescoordinator/assignstudentToLecturer
        [HttpPost]
        [Route("assignstudentToLecturer")]
        public async Task<ActionResult> AssignStudentToLecturer([FromBody] AssignStudentToLecturerRequest assignStudentToLecturer)
        {
            var result = await _siwesCoRepository.AssignStudentToLecturer(assignStudentToLecturer);

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
        /// Get A Student Log Books
        /// </summary>
        /// Get: api/siwescoordinator/studentlogbook/1
        [HttpGet]
        [Route("studentlogbook/{studentId}")]
        public async Task<ActionResult> GetStudentLogBooks(int studentId)
        {
            var result = await _siwesCoRepository.GetStudentLogBooks(studentId);

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
        /// Get A Student Log Book
        /// </summary>
        /// Get: api/siwescoordinator/studentlogbook/1/1
        [HttpGet]
        [Route("studentlogbook/{studentId}/{logBookId}")]
        public async Task<ActionResult> GetStudentLogBook(int studentId, int logBookId)
        {
            var result = await _siwesCoRepository.GetStudentLogBook(studentId, logBookId);

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
        /// Update SiwesCo FOR AN Siwes Application
        /// </summary>
        /// Post: api/siwesco/update/1
        [HttpPost]
        [Route("update/{siwesCoId}")]
        public async Task<ActionResult> UpdateSiwesCo(int siwesCoId, SiwesCoordinatorRequest siwesCoordinatorRequest)
        {
            var result = await _siwesCoRepository.UpdateSiwesCo(siwesCoId, siwesCoordinatorRequest);

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
