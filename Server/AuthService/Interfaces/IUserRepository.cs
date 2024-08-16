using DataLayer.Models;

namespace AuthService.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByUsernameAsync(string username);
    }
}
