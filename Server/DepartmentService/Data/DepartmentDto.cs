using DataLayer.Models;

namespace DepartmentService.Data
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<TeacherDto> Teachers { get; set; } = [];
    }
}
