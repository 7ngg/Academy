using DataLayer.Models;

namespace FacultyService.Repositories.Interfaces
{
    public interface IFacultyRepository
    {
        Task Save();
        Task<IEnumerable<Faculty>> GetAll();
        Task<Faculty> GetById(Guid id);
        Task RemoveAsync(Guid id);
        Task AddAsync(Faculty faculty);
    }
}
