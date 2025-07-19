namespace BackendAPI.Dtos
{
    public class GetUsersResponseDto
    {
        public List<UserTableItemDto> dataSource { get; set; }

        public int page { get; set; }

        public int pageSize { get; set; }

        public int totalCount { get; set; }
    }

    public class UserTableItemDto
    {
        public string userId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public RoleDto Role { get; set; }

        public string username { get; set; }

        public List<PermissionReturnDto> Permissions { get; set; }

        public string createdDate { get; set; }
    }
}
