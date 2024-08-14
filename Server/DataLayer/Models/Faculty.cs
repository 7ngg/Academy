#nullable disable

namespace DataLayer.Models
{
    public class Faculty
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public ICollection<Group> Groups { get; set; }
    }
}
