using DataLayer.Contexts;
using DataLayer.Models;
using DepartmentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DepartmentService.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AcademyContext _context;

        public DepartmentRepository(AcademyContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departmnets.AddAsync(department);
        }

        public void Delete(Department department)
        {
            _context.Remove(department);
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            return await _context.Departmnets
                .Include(d => d.Teachers)
                .ThenInclude(t => t.User)
                .ToListAsync();
        }

        public async Task<Department?> GetById(Guid id)
        {
            return await _context.Departmnets
                .Include(d => d.Teachers)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 1;
        }
    }
}
