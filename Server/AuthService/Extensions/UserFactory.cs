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
                Password = passwordHash,
                Email = email,
                Name = name,
                Surname = surname,
            };
        }
    }

}
