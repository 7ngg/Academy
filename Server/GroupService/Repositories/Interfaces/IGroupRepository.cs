using DataLayer.Models;
using GroupService.Data.Dtos;

namespace GroupService.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        Task AddAsync(Group group);
        Task<IEnumerable<Group>> GetAllAsync();
        Task<Group?> GetByIdAsync(Guid id);
        Task DeleteAsync(Group group);
        Task SaveAsync();
    }
}
