#nullable disable

namespace DataLayer.Models
{
    public class Group
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public ICollection<Student> Students { get; set; } = [];
    }
}
