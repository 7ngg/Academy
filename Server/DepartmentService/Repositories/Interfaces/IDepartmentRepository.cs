using DataLayer.Models;

namespace DepartmentService.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<bool> SaveAsync();
        Task<IEnumerable<Department>> GetAll();
        Task<Department?> GetById(Guid id);
        Task AddAsync(Department department);
        void Delete(Department department);
    }
}
