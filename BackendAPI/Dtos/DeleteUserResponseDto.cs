namespace BackendAPI.Dtos
{
    public class DeleteUserResponseDto
    {
        public StatusDto status { get; set; }
        public DeleteResultDto data { get; set; }
    }

    public class DeleteResultDto
    {
        public bool result { get; set; }

        public string message { get; set; }
    }
}
