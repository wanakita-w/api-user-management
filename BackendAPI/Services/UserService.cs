using BackendAPI.Data;
using BackendAPI.Dtos;
using BackendAPI.Models.Damain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace BackendAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST api/user
        public async Task<UserDataDto> CreateUserAsync(AddUserRequestDto request)
        {
            // 1. เตรียม User object
            var user = new User
            {
                //กำหนด userId ของ user ให้มีค่าเดียวกับ userId ที่ถูกส่งเข้ามาใน request
                userId = request.userId,
                firstName = request.firstName,
                lastName = request.lastName,
                email = request.email,
                phone = request.phone,
                username = request.username,
                password = request.password,
                roleId = request.roleId,
                Permissions = request.Permissions.Select(p => new UserPermission
                {
                    permissionId = p.permissionId,
                    IsReadable = p.IsReadable,
                    IsWritable = p.IsWritable,
                    IsDeletable = p.IsDeletable
                }).ToList()
            };

            // 2. บันทึกลงฐานข้อมูล
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 3. ดึง roleName จาก Role
            var role = await _context.Roles.FindAsync(user.roleId);

            // 4. ดึง permissionName จาก Permissions (แก้ปัญหา EF ไม่โหลด navigation)
            var permissionIds = user.Permissions.Select(p => p.permissionId).ToList();
            var permissionMap = await _context.Permissions
                .Where(p => permissionIds.Contains(p.permissionId))
                .ToDictionaryAsync(p => p.permissionId, p => p.permissionName);

            // 5. สร้าง DTO สำหรับ return
            return new UserDataDto
            {
                userId = user.userId,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                phone = user.phone,
                username = user.username,
                Role = new RoleDto
                {
                    roleId = user.roleId,
                    roleName = role?.roleName
                },
                Permissions = user.Permissions.Select(p => new PermissionReturnDto
                {
                    permissionId = p.permissionId,
                    permissionName = permissionMap.ContainsKey(p.permissionId) ? permissionMap[p.permissionId] : ""
                }).ToList()
            };
        }

        // GET api/users/{id}
        public async Task<UserDataDto?> GetUserByIdAsync(string userId) //ใช้ UserDataDto? (nullable) เพื่อบอกว่าอาจหาไม่เจอ แล้วคืน null กลับไปได้
        {
            // 1. ดึงข้อมูล User พร้อม Role และ Permissions (Include)
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Permissions)
                    .ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(u => u.userId == userId);

            if (user == null)
                return null; // ให้ Controller จัดการกรณีไม่พบ

            // 2. สร้าง DTO เพื่อตอบกลับ
            return new UserDataDto
            {
                userId = user.userId,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                phone = user.phone,
                username = user.username,
                Role = new RoleDto
                {
                    roleId = user.roleId,
                    roleName = user.Role?.roleName
                },
                Permissions = user.Permissions.Select(p => new PermissionReturnDto
                {
                    permissionId = p.permissionId,
                    permissionName = p.Permission?.permissionName ?? ""
                }).ToList()
            };
        }

        // GET api/users/DataTable
        public async Task<GetUsersResponseDto> GetFilteredUsersAsync(UserFilterRequestDto request)
        {
            // Console.WriteLine($"order = {request.orderBy}, direction = {request.orderDirection}");

            // 1. เตรียม Queryable
            var query = _context.Users
                .Include(u => u.Role)
                .Include(u => u.Permissions)
                    .ThenInclude(up => up.Permission)
                .AsQueryable();

            // 2. Search
            if (!string.IsNullOrEmpty(request.search))
            {
                var keyword = request.search.ToLower();
                query = query.Where(u =>
                    u.firstName.ToLower().Contains(keyword) ||
                    u.lastName.ToLower().Contains(keyword) ||
                    u.email.ToLower().Contains(keyword));
            }

            // 3. Total ก่อน paging
            var totalCount = await query.CountAsync();

            // 4. Sorting
            if (!string.IsNullOrEmpty(request.orderBy))
            {
                var order = request.orderBy.ToLower(); //sort by (firt, last, email)
                var direction = request.orderDirection?.ToLower() ?? "asc"; //(asc/ desc)

                // รองรับเฉพาะ field ที่อนุญาต
                if (order == "firstname")
                {
                    query = direction == "desc"
                        ? query.OrderByDescending(u => u.firstName)
                        : query.OrderBy(u => u.firstName);
                }
                else if (order == "lastname")
                {
                    query = direction == "desc"
                        ? query.OrderByDescending(u => u.lastName)
                        : query.OrderBy(u => u.lastName);
                }
                else if (order == "email")
                {
                    query = direction == "desc"
                        ? query.OrderByDescending(u => u.email)
                        : query.OrderBy(u => u.email);
                }
                else
                {
                    query = direction == "desc"
                        ? query.OrderByDescending(u => u.userId)
                        : query.OrderBy(u => u.userId);
                }
            }

            // 5. Paging
            var pageNumber = request.pageNumber ?? 1;
            var pageSize = request.pageSize ?? 10;
            var skip = (pageNumber - 1) * pageSize;

            var users = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            // 6. Map to DTO
            var dataSource = users.Select(user => new UserTableItemDto
            {
                userId = user.userId,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                username = user.username,
                createdDate = user.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Role = new RoleDto
                {
                    roleId = user.roleId,
                    roleName = user.Role?.roleName
                },
                Permissions = user.Permissions.Select(p => new PermissionReturnDto
                {
                    permissionId = p.permissionId,
                    permissionName = p.Permission?.permissionName ?? ""
                }).ToList()
            }).ToList();

            // 7. Return
            return new GetUsersResponseDto
            {
                dataSource = dataSource,
                page = pageNumber,
                pageSize = pageSize,
                totalCount = totalCount
            };
        }


        public async Task<UserDataDto> UpdateUserAsync(string userId, EditUserRequestDto request)
        {
            // 1. ค้นหา user เดิม
            var user = await _context.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.userId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            // 2. อัปเดตข้อมูลหลัก
            user.firstName = request.firstName;
            user.lastName = request.lastName;
            user.email = request.email;
            user.phone = request.phone;
            user.roleId = request.roleId;
            user.username = request.username;
            user.password = request.password;

            // 3. ลบ permissions เดิม
            _context.UserPermissions.RemoveRange(user.Permissions);

            // 4. เพิ่ม permissions ใหม่
            user.Permissions = request.Permissions.Select(p => new UserPermission
            {
                userId = user.userId,
                permissionId = p.permissionId,
                IsReadable = p.IsReadable,
                IsWritable = p.IsWritable,
                IsDeletable = p.IsDeletable
            }).ToList();

            await _context.SaveChangesAsync();

            // 5. ดึง roleName
            var role = await _context.Roles.FindAsync(user.roleId);

            // 6. ดึง permission names
            var permissionIds = user.Permissions.Select(p => p.permissionId).ToList();
            var permissionNames = await _context.Permissions
                .Where(p => permissionIds.Contains(p.permissionId))
                .ToDictionaryAsync(p => p.permissionId, p => p.permissionName);

            // 7. Return DTO
            return new UserDataDto
            {
                userId = user.userId,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                phone = user.phone,
                username = user.username,
                Role = new RoleDto
                {
                    roleId = user.roleId,
                    roleName = role?.roleName
                },
                Permissions = user.Permissions.Select(p => new PermissionReturnDto
                {
                    permissionId = p.permissionId,
                    permissionName = permissionNames.ContainsKey(p.permissionId)
                        ? permissionNames[p.permissionId]
                        : ""
                }).ToList()
            };
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            // 1. ค้นหา user จาก userId
            var user = await _context.Users
                .Include(u => u.Permissions) // รวม permissions ไปด้วย
                .FirstOrDefaultAsync(u => u.userId == userId);

            if (user == null)
            {
                return false; // ไม่เจอ user
            }

            // 2. ลบ UserPermission ก่อน (EF Core จะจัดการความสัมพันธ์ให้ก็จริง แต่เราจัดการเองชัดเจนกว่า)
            _context.UserPermissions.RemoveRange(user.Permissions);

            // 3. ลบ user
            _context.Users.Remove(user);

            // 4. บันทึกการเปลี่ยนแปลง
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
