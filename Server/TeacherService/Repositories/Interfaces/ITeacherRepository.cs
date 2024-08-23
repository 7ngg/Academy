using DataLayer.Models;

namespace TeacherService.Repositories.Interfaces
{
    public interface ITeacherRepository
    {
        Task<bool> SaveAsync();
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<Teacher?> GetByIdAsync(Guid id);
        Task AddAsync(Teacher teacher);
        public void Delete(Teacher teacher);
    }
}
