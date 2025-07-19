using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Dtos
{
    public class AddUserRequestDto
    {
        [Required]
        public string userId { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public string email { get; set; }

        public string? phone { get; set; }

        [Required]
        public string roleId { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public List<PermissionRWDDto> Permissions { get; set; }
    }
}
