using System.ComponentModel.DataAnnotations;

namespace Role_Management_BackEnd.Models
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
