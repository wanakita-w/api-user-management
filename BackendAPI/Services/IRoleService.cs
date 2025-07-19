using BackendAPI.Dtos;

namespace BackendAPI.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();
    }
}
