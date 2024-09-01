using DataLayer.Models;
using System.Linq.Expressions;

namespace AuthService.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> SaveAsync();
        Task AddAsync(User user);
        Task<User> GetByUsernameAsync(string username);
        IEnumerable<User> Filter(Expression<Func<User, bool>> expression);
    }
}
