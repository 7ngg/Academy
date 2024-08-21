using DataLayer.Contexts;
using DataLayer.Models;
using FacultyService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FacultyService.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly AcademyContext _context;

        public FacultyRepository(AcademyContext context)
        {
            _context = context;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Faculty>> GetAll()
        {
            var faculties = await _context.Faculties
                .Include(f => f.Groups)
                .ToListAsync();

            return faculties;
        }

        public async Task<Faculty> GetById(Guid id)
        {
            var faculty = await _context.Faculties
                .Include(f => f.Groups)
                .FirstOrDefaultAsync(f => f.Id == id);

            return faculty;
        }

        public async Task RemoveAsync(Guid id)
        {
            var faculty = await _context.Faculties.FindAsync(id);

            if (faculty is null)
            {
                // TODO: Custom exception
                throw new NullReferenceException(nameof(faculty));
            }

            try
            {
                _context.Remove(faculty);
            }
            catch (Exception)
            {
                throw;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Faculty faculty)
        {
            ArgumentNullException.ThrowIfNull(faculty);

            await _context.AddAsync(faculty);
        }
    }
}
