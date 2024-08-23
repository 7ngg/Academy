using DataLayer.Contexts;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using TeacherService.Repositories.Interfaces;

namespace TeacherService.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AcademyContext _context;

        public TeacherRepository(AcademyContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Department)
                .Include(t => t.Groups)
                .ToListAsync();
        }

        public async Task<Teacher?> GetByIdAsync(Guid id)
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Groups)
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void Delete(Teacher teacher)
        {
            _context.Teachers.Remove(teacher);
        }
    }
}
