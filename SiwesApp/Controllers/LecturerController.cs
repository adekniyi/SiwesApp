using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    }
}
