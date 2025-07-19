using BackendAPI.Data;
using BackendAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Services
{
    public class RoleService: IRoleService
    {
        private readonly ApplicationDbContext _context;
        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _context.Roles
                .Select(r => new RoleDto
                {
                    roleId = r.roleId,
                    roleName = r.roleName
                })
                .ToListAsync();

            return roles;
        }
    }
}
