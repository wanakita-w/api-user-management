namespace BackendAPI.Dtos
{
    public class GetRoleResponseDto
    {
        public StatusDto status { get; set; }
        public List<RoleDto> data { get; set; }
    }
}
