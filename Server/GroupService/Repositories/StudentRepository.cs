using DataLayer.Contexts;
using DataLayer.Models;
using GroupService.Repositories.Interfaces;

namespace GroupService.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AcademyContext _context;

        public StudentRepository(AcademyContext context)
        {
            _context = context;
        }

        public async Task<Student> GetByIdAsync(Guid id)
        {
            return await _context.Students.FindAsync(id);
        }
    }
}
