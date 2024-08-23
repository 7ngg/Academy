using DataLayer.Contexts;
using DataLayer.Models;
using GroupService.Data.Dtos;
using GroupService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GroupService.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AcademyContext _context;

        public GroupRepository(AcademyContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Group group)
        {
            try
            {
                await _context.Groups.AddAsync(group);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            var groups = await _context.Groups
                .Include(g => g.Faculty)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.User)
                .Include(g => g.Students)
                .ThenInclude(s => s.User)
                .ToListAsync();


            return groups;
        }

        public async Task<Group?> GetByIdAsync(Guid id)
        {
            var group = await _context.Groups
                .Include(g => g.Faculty)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.User)
                .Include(g => g.Students)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            return group;
        }

        public async Task DeleteAsync(Group group)
        {
            try
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
