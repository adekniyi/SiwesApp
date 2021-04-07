using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiwesApp.Dtos.SiwesCoOrdinatotDto;
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
    public class SiwesCoController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISiwesCoordinatorRepo _siwesCoRepository;

        public SiwesCoController(UserManager<User> userManager, ISiwesCoordinatorRepo siwesCoRepository)
        {
            _userManager = userManager;
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
        /// Get All SiwesCo FOR AN Siwes Application
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
        /// Get: api/siwesCoordinator/1
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

    }
}
