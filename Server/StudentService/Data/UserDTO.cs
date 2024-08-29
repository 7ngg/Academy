using DataLayer.Models;

namespace StudentService.Data
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Roles Role { get; set; } = Roles.STUDENT;
    }
}
