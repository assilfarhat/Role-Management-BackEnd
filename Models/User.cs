using System.ComponentModel.DataAnnotations;

namespace Role_Management_BackEnd.Models
{
    public enum RoleType
    {
        Admin,
        Moderator,
        User,
        Guest
        // Add more roles as needed
    }
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? UserEmail { get; set; }
        public string? Token { get; set; }
        public RoleType? Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
