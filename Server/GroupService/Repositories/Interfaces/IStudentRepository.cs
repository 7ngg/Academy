using DataLayer.Models;

namespace GroupService.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> GetByIdAsync(Guid id);
    }
}
