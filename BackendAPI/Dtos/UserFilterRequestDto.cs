namespace BackendAPI.Dtos
{
    public class UserFilterRequestDto
    {
        public string? orderBy { get; set; }

        public string? orderDirection { get; set; }

        public int? pageNumber { get; set; }

        public int? pageSize { get; set; }

        public string? search { get; set; }
    }
}
