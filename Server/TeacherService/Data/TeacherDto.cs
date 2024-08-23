using DataLayer.Models;

namespace TeacherService.Data
{
    public class TeacherDto
    {
        public Guid Id { get; set; }

        public ICollection<GroupDto> Groups { get; set; } = [];

        public Guid DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }

        public Guid UserId { get; set; }
        public UserDto User { get; set; }
    }
}
