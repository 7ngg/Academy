using DataLayer.Models;

namespace GroupService.Repositories.Interfaces
{
    public interface ITeacherRepository
    {
        Task<Teacher> GetByIdAsync(Guid id);
    }
}
