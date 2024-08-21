using DataLayer.Models;

namespace DepartmentService.Data
{
    public class TeacherDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserDto User { get; set; }
    }
}
