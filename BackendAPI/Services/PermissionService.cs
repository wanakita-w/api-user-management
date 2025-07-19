using BackendAPI.Data;
using BackendAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Services
{
    public class PermissionService: IPermissionService
    {
        private readonly ApplicationDbContext _context;
        public PermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionReturnDto>> GetAllPermissionsAsync()
        {
            var permissions = await _context.Permissions
                .Select(p => new PermissionReturnDto
                {
                    permissionId = p.permissionId,
                    permissionName = p.permissionName
                })
                .ToListAsync();

            return permissions;
        }
    }
}
