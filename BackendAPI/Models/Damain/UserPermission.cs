namespace BackendAPI.Models.Damain
{
    public class UserPermission
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string permissionId { get; set; }

        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeletable { get; set; }

        public User User { get; set; }
        public Permission Permission { get; set; }
    }
}
