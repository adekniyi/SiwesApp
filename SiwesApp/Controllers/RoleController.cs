using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiwesApp.Data;
using SiwesApp.Dtos.All;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiwesApp.Controllers
{
    [Authorize(Roles = "SiwesAdmin,SiwesCoordinator")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleManagementRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDataContext _dataContext;

        public RoleController(IRoleRepository roleManagementRepository, IMapper mapper, ApplicationDataContext dataContext)
        {
            _roleManagementRepository = roleManagementRepository;
            _mapper = mapper;
            _dataContext = dataContext;
        }


        /// <summary>
        /// CREATE ROLE
        /// </summary>
        [HttpPost("Create")]
        public async Task<ActionResult> PostRoles(List<RoleRequest> roles)
        {
            var result = await _roleManagementRepository.CreateRoles(_mapper.Map<List<Role>>(roles));

            if (result.StatusCode == Helpers.Success)
            {
                result.ObjectValue = _mapper.Map<List<RoleResponse>>(((List<Role>)result.ObjectValue));
                return StatusCode(StatusCodes.Status200OK, result.ObjectValue);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }

        /// <summary>
        /// GET A ROLE BY ROLEID = ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoles(int id)
        {
            var result = await _roleManagementRepository.GetRoles(id);

            if (result.StatusCode == Helpers.Success)
            {
                var role = (Role)result.ObjectValue;
                int userCountInRole = role.UserRoles.Count;
                var roleResponse = _mapper.Map<RoleResponse>(role);
                roleResponse.NumberOfUsersAssignedToRole = userCountInRole;
                result.ObjectValue = roleResponse;
                return StatusCode(StatusCodes.Status200OK, result.ObjectValue);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }

        /// <summary>
        /// DELETE ROLES
        /// </summary>
        [HttpPost("Delete")]
        public async Task<ActionResult> DeleteRoles(List<RoleResponse> roles)
        {
            var result = await _roleManagementRepository.DeleteRoles(roles);

            if (result.StatusCode == Helpers.Success)
            {
                result.ObjectValue = _mapper.Map<List<RoleResponse>>(((List<Role>)result.ObjectValue));
                return StatusCode(StatusCodes.Status200OK, result.ObjectValue);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }


        /// <summary>
        /// UPDATE ROLES IN THE SYSTEM
        /// </summary>
        [HttpPost("Update")]
        public async Task<ActionResult> PutRoles(List<RoleResponse> roleResponses)
        {
            var result = await _roleManagementRepository.UpdateRoles(roleResponses);

            if (result.StatusCode == Helpers.Success)
            {
                result.ObjectValue = _mapper.Map<List<RoleResponse>>(((List<Role>)result.ObjectValue));
                return StatusCode(StatusCodes.Status200OK, result.ObjectValue);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }

        /// <summary>
        /// ASSIGN ROLES
        /// </summary>
        [HttpPost("Users/Assign")]
        public async Task<ActionResult> PostAssignRolesToUser([FromBody] RoleUserAssignmentRequest roleAssignmentRequest)
        {
            var dbTransaction = await _dataContext.Database.BeginTransactionAsync();
            var result = await _roleManagementRepository.AssignRolesToUser(roleAssignmentRequest);

            if (result.StatusCode == Helpers.Success)
            {
                await dbTransaction.CommitAsync();
                return StatusCode(StatusCodes.Status200OK, result.ObjectValue);
            }
            else
            {
                await dbTransaction.RollbackAsync();
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }

    }
}
