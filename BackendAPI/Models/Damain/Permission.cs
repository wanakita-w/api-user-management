namespace BackendAPI.Models.Damain
{
    public class Permission
    {
        public string permissionId { get; set; }
        public string permissionName { get; set; }

        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
