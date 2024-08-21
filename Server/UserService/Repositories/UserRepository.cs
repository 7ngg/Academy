using DataLayer.Contexts;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AcademyContext _context;

        public UserRepository(AcademyContext context)
        {
            _context = context;
        }

        public async Task Create(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            await _context.Users.AddAsync(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() >= 1;
        }
    }
}
