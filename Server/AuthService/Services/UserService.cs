using AuthService.Data.DTOs;
using AuthService.Extensions;
using AuthService.Infrastructure;
using AuthService.Interfaces;
using DataLayer.Models;

namespace AuthService.Services
{
    public class UserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository, ITokenService tokenService)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task SignUp(SignUpDTO request)
        {
            string passwordHash = _passwordHasher.Generate(request.Password);

            var user = UserFactory.Create(
                Guid.NewGuid(),
                request.Username, passwordHash,
                request.Email, request.Name, request.Surname);

            await _userRepository.AddAsync(user);
        }

        public async Task<string> SignIn(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                // Exception
            }

            var passwordCheck = _passwordHasher.Verify(password, user.Password);

            if (!passwordCheck)
            {
                // Exception
            }

            var token = _tokenService.Generate(user);

            return token;
        }
    }
}
