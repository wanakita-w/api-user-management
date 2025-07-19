namespace BackendAPI.Dtos
{
    public class UserDataDto
    {
        public string userId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string? phone { get; set; }

        public RoleDto Role { get; set; }

        public string username { get; set; }

        public List<PermissionReturnDto> Permissions { get; set; }
    }
}
