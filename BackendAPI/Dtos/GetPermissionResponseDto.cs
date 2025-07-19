namespace BackendAPI.Dtos
{
    public class GetPermissionResponseDto
    {
        public StatusDto status { get; set; }
        public List<PermissionReturnDto> data { get; set; }
    }
}
