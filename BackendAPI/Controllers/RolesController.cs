using BackendAPI.Dtos;
using BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController: ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/roles
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetRoleResponseDto))]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();

            return Ok(new GetRoleResponseDto
            {
                status = new StatusDto { code = "200", description = "Success" },
                data = roles
            });
        }
    }
}
