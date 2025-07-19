namespace BackendAPI.Models.Damain
{
    public class Role
    {
        public string roleId { get; set; }

        public string roleName { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
