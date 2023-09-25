using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Role_Management_BackEnd.Models
{

    public class Roles
    {
        [Key]
        public int Id { get; set; } 
        public string RoleName { get; set; }

        [JsonIgnore]
        public ICollection<Permissions> Permissions { get; } = new List<Permissions>();

       /* [JsonIgnore]
        public ICollection<User> Users { get; } = new List<User>();*/

    }
}
