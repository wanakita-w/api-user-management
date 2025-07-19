namespace BackendAPI.Dtos
{
    public class GetUserByIdResponseDto
    {
        public StatusDto status { get; set; }
        public UserDataDto data { get; set; }
    }
}
