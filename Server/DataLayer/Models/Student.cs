using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
