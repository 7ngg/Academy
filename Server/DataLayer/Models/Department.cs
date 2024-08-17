#nullable disable

namespace DataLayer.Models 
{
    public class Department
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public ICollection<Teacher> Teachers { get; set; } = [];
    }
}
