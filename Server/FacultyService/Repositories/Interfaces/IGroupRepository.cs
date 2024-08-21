using DataLayer.Models;

namespace FacultyService.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        Task<Group> GetById(Guid id);
    }
}
