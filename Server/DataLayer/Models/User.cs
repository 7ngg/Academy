#nullable disable

namespace DataLayer.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Roles Role { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
