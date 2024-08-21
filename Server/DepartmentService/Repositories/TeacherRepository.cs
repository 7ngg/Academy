using DataLayer.Contexts;
using DataLayer.Models;
using DepartmentService.Repositories.Interfaces;

namespace DepartmentService.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AcademyContext _context;

        public TeacherRepository(AcademyContext context)
        {
            _context = context;
        }

        public async Task<Teacher> GetById(Guid id)
        {
            return await _context.Teachers.FindAsync(id);
        }
    }
}
