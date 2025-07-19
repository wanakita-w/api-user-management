using BackendAPI.Dtos;
using BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController: ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // 1. Create User
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddUserResponseDto))]
        public async Task<IActionResult> CreateUser([FromBody] AddUserRequestDto request)
        {
            var user = await _userService.CreateUserAsync(request);
            return Ok(new AddUserResponseDto
            {
                status = new StatusDto { code = "200", description = "User created successfully" },
                data = new List<UserDataDto> { user }
            });
        }


        // 2. Get User by Id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserByIdResponseDto))]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new GetUserByIdResponseDto
                {
                    status = new StatusDto { code = "404", description = "User not found" },
                    data = null
                });
            }

            return Ok(new GetUserByIdResponseDto
            {
                status = new StatusDto { code = "200", description = "Success" },
                data = user
            });
        }


       
        // 3. Get Filtered Users
        [HttpPost("DataTable")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUsersResponseDto))]
        public async Task<IActionResult> GetUsers([FromBody] UserFilterRequestDto filter)
        {
            var result = await _userService.GetFilteredUsersAsync(filter);
            return Ok(result); // ไม่มี status เพราะ GetUsersResponseDto ไม่มี status field
        }


        // 4. Update User
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditUserResponseDto))]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] EditUserRequestDto request)
        {
            var updated = await _userService.UpdateUserAsync(id, request);
            if (updated == null)
            {
                return NotFound(new EditUserResponseDto
                {
                    status = new StatusDto { code = "404", description = "User not found" },
                    data = null
                });
            }

            return Ok(new EditUserResponseDto
            {
                status = new StatusDto { code = "200", description = "User updated successfully" },
                data = updated
            });
        }

        // 5. Delete User
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteUserResponseDto))]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound(new DeleteUserResponseDto
                {
                    status = new StatusDto { code = "404", description = "User not found" },
                    data = new DeleteResultDto
                    {
                        result = false,
                        message = "User deletion failed"
                    }
                });
            }

            return Ok(new DeleteUserResponseDto
            {
                status = new StatusDto { code = "200", description = "User deleted successfully" },
                data = new DeleteResultDto
                {
                    result = true,
                    message = "User was deleted"
                }
            });
        }
    }
}
