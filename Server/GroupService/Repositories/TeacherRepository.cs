using DataLayer.Contexts;
using DataLayer.Models;
using GroupService.Repositories.Interfaces;

namespace GroupService.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AcademyContext _context;

        public TeacherRepository(AcademyContext context)
        {
            _context = context;
        }

        public async Task<Teacher> GetByIdAsync(Guid id)
        {
            return await _context.Teachers.FindAsync(id);
        }
    }
}
