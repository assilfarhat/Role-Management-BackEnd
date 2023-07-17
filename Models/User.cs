using System.ComponentModel.DataAnnotations;

namespace Role_Management_BackEnd.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FisrtName { get; set; }
        public string LasrName { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
