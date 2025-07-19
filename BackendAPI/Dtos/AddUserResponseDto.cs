namespace BackendAPI.Dtos
{
    public class AddUserResponseDto
    {
        public StatusDto status { get; set; }
        public List<UserDataDto> data { get; set; }
    }
}
