#nullable disable

namespace DataLayer.Models
{
    public class Schedule 
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TeacherId { get; set; }
        public User Teacher { get; set; }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }
    }
}
