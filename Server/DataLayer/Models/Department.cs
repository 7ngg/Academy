#nullable disable

namespace DataLayer.Models 
{
    public class Departmnet
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public ICollection<User> Teachers { get; set; } = [];
    }
}
