using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IStudentRepo _studentRepository;
        //private readonly IMapper _mapper;
        //private readonly IConfiguration _configuration;

        public StudentController(UserManager<User> userManager, IStudentRepo studentRepository, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _studentRepository = studentRepository;
            //_mapper = mapper;
            //_configuration = configuration;
        }


        /// <summary>
        /// CREATE A STUDENT FOR AN Siwes Application
        /// </summary>
        /// POST: api/student
        [HttpPost]
        public async Task<ActionResult> CreateStudent([FromForm] StudentRequest studentRequest)
        {
            var result = await _studentRepository.CreateStudent(studentRequest);

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
        /// Get All STUDENTs FOR AN Siwes Application
        /// </summary>
        /// Get: api/student
        [HttpGet]
        public async Task<ActionResult> GetAllStudents()
        {
            var result = await _studentRepository.GetAllStudents();

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
        /// Get One STUDENT FOR AN Siwes Application
        /// </summary>
        /// Get: api/student/1
        [HttpGet]
        [Route("{studentId}")]
        public async Task<ActionResult> GetOneStudent(int studentId)
        {
            var result = await _studentRepository.GetOneStudent(studentId);

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
        /// CREATE A Placement For STUDENT 
        /// </summary>
        /// POST: api/student/placement
        [HttpPost]
        [Route("placement")]
        public async Task<ActionResult> StudentPlacement([FromForm] PlacementRequestDto placementRequest)
        {
            var result = await _studentRepository.StudentPlacement(placementRequest);

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
