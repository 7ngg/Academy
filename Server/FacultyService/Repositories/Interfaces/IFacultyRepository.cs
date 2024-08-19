using FacultyService.Data;

namespace FacultyService.Repositories.Interfaces
{
    public interface IFacultyRepository
    {
        Task<IEnumerable<FacultyDto>> GetAll();
        Task<FacultyDto> GetById(Guid id);
    }
}
