using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiwesApp.Dtos.IndustrialSupervisorDto;
using SiwesApp.Interfaces;
using SiwesApp.Utils;
using System.Threading.Tasks;

namespace SiwesApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IndustrialSupervisorController : ControllerBase
    {
        private readonly IIndustrialSupervisorRepo _industrialSupervisorRepository;

        public IndustrialSupervisorController(IIndustrialSupervisorRepo industrialSupervisorRepository)
        {
            _industrialSupervisorRepository = industrialSupervisorRepository;
        }

        /// <summary>
        /// CREATE Industrial Supervisor FOR A Siwes Application
        /// </summary>
        /// POST: api/industrialSupervisor
        [HttpPost]
        [Authorize(Roles = "SiwesAdmin")]
        public async Task<ActionResult> CreateIndustrialSupervisor([FromForm] IndustrialSupervisorRequest industrialSupervisorRequest)
        {
            var result = await _industrialSupervisorRepository.CreateIndustrialSupervisor(industrialSupervisorRequest);

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
        /// Get All Industrial Supervisors FOR AN Siwes Application
        /// </summary>
        /// Get: api/industrialSupervisor
        [HttpGet]
        public async Task<ActionResult> GetAllIndustrialSupervisors()
        {
            var result = await _industrialSupervisorRepository.GetAllIndustrialSupervisors();

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
        /// Get One Industrial Supervisor FOR AN Siwes Application
        /// </summary>
        /// Get: api/industrialSupervisor/1
        [HttpGet]
        [Route("{industrialSupervisorId}")]
        public async Task<ActionResult> GetOneIndustrialSupervisor(int industrialSupervisorId)
        {
            var result = await _industrialSupervisorRepository.GetOneIndustrialSupervisor(industrialSupervisorId);

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
