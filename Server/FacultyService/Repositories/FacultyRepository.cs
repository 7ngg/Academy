using DataLayer.Contexts;
using FacultyService.Data;
using FacultyService.Infrastructure;
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

        public async Task<IEnumerable<FacultyDto>> GetAll()
        {
            var faculties = await _context.Faculties
                .Include(f => f.Groups)
                .ToListAsync();

            var facultyDtos = faculties.Select(f => FacultyDtoProvider.Generate(f));

            return facultyDtos;
        }

        public async Task<FacultyDto> GetById(Guid id)
        {
            var faculty = await _context.Faculties
                .Include(f => f.Groups)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (faculty is null)
            {
                // TODO: Custom exception
                throw new NullReferenceException(nameof(faculty));
            }

            return FacultyDtoProvider.Generate(faculty);
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
    }
}
