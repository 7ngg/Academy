using DataLayer.Contexts;
using DataLayer.Models;
using FacultyService.Repositories.Interfaces;

namespace FacultyService.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AcademyContext _context;

        public GroupRepository(AcademyContext context)
        {
            _context = context;
        }

        public async Task<Group> GetById(Guid id)
        {
            return await _context.Groups.FindAsync(id);
        }
    }
}
