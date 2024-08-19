using DataLayer.Models;
using GroupService.Data;

namespace GroupService.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        Task AddAsync(Group group);
        Task<IEnumerable<GroupDTO>> GetAllAsync();
        Task<Group?> GetByIdAsync(Guid id);
        Task RemoveAsync(Group group);
        Task SaveAsync();
    }
}
