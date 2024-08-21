using DataLayer.Models;

namespace UserService.Data
{
    public class UserCreateDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Roles Role { get; set; }
    }
}
