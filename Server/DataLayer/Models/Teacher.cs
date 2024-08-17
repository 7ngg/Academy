using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Teacher
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Group> Groups { get; set; } = [];

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
