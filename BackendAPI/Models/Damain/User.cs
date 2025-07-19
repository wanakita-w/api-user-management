namespace BackendAPI.Models.Damain
{
    public class User
    {
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string? phone { get; set; }
        public string roleId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime CreatedDate { get; set; }
        public Role Role { get; set; }
        public ICollection<UserPermission> Permissions { get; set; }
    }
}
