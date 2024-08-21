using DataLayer.Models;

namespace DepartmentService.Repositories.Interfaces
{
    public interface ITeacherRepository
    {
        Task<Teacher> GetById(Guid id);
    }
}
