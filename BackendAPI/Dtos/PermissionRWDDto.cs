using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Dtos
{
    public class PermissionRWDDto
    {

        [Required]
        public string permissionId { get; set; }

        [Required]
        public bool IsReadable { get; set; }

        [Required]
        public bool IsWritable { get; set; }

        [Required]
        public bool IsDeletable { get; set; }
    }
}
