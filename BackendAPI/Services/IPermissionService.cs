using BackendAPI.Dtos;

namespace BackendAPI.Services
{
    public interface IPermissionService
    {
        Task<List<PermissionReturnDto>> GetAllPermissionsAsync();
    }
}
