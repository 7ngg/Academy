using DataLayer.Models;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Save();
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid id);
        Task Create(User user);
        void Delete(User user);
    }
}
