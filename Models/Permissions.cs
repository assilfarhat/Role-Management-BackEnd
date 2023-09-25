using System.ComponentModel.DataAnnotations;

namespace Role_Management_BackEnd.Models
{
    public class Permissions
    {
        [Key]
        public int Id { get; set; }
        public string PermissionName { get; set; }

        public ICollection<Roles> Roles { get; } = new List<Roles>();
    }
}
