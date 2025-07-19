using BackendAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Services
{
    public interface IUserService
    {
        // POST api/user
        Task<UserDataDto> CreateUserAsync(AddUserRequestDto request);
        Task<UserDataDto?> GetUserByIdAsync(string userId);
        Task<GetUsersResponseDto> GetFilteredUsersAsync(UserFilterRequestDto request);
        Task<UserDataDto?> UpdateUserAsync(string userId, EditUserRequestDto request);
        Task<bool> DeleteUserAsync(string userId);
    }
}
