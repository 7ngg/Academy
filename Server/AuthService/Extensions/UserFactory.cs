using DataLayer.Models;

namespace AuthService.Extensions
{
    public static class UserFactory
    {
        public static User Create(Guid userId,
            string username, string passwordHash,
            string email, string name, string surname)
        {
            return new User
            {
                Id = userId,
                Username = username,
                PasswordHash = passwordHash,
                Email = email,
                FirstName = name,
                LastName = surname
            };
        }
    }

}
