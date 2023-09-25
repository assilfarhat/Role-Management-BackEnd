namespace Role_Management_BackEnd.Models
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public Roles Role { get; set; }

        public int PermissionId { get; set; }
        public Permissions Permission { get; set; }
    }
}
