using FacultyService.Data;

namespace FacultyService.Repositories.Interfaces
{
    public interface IFacultyRepository
    {
        Task Save();
        Task<IEnumerable<FacultyDto>> GetAll();
        Task<FacultyDto> GetById(Guid id);
        Task RemoveAsync(Guid id);
    }
}
