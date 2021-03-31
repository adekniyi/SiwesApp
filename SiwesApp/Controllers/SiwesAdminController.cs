using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiwesApp.Dtos.Authentication;
using SiwesApp.Dtos.SiwesAdmin;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Controllers
{
    [Authorize(Roles = "SiwesAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SiwesAdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISiwesAdminRepo _siwesAdminRepository;
        private readonly IMapper _mapper;

        public SiwesAdminController(UserManager<User> userManager, ISiwesAdminRepo siwesAdminRepository, IMapper mapper)
        {
            _userManager = userManager;
            _siwesAdminRepository = siwesAdminRepository;
            _mapper = mapper;
        }



        /// <summary>
        /// CREATE A Siwes ADMIN
        /// </summary>
        // POST: api/SiwesAdmin
        [HttpPost]
        //[Route("admin")]
        public async Task<ActionResult> CreateSiwesAdmin([FromForm] SiwesAdminRequest siwesAdminRequest)
        {
            var result = await _siwesAdminRepository.CreateSiwesAdmin(siwesAdminRequest);

            if (result.StatusCode == Helpers.Success)
            {
                result.ObjectValue = _mapper.Map<UserToReturn>((User)result.ObjectValue);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
    }
}
