using DataLayer.Models;

namespace TeacherService.Data
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Roles Role { get; set; }
    }
}
