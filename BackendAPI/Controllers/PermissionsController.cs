using BackendAPI.Dtos;
using BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/permissions")]
    public class PermissionsController: ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        // GET: api/permissions
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPermissionResponseDto))]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();

            return Ok(new GetPermissionResponseDto
            {
                status = new StatusDto { code = "200", description = "Success" },
                data = permissions
            });
        }
    }
}
