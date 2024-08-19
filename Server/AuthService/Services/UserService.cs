using AuthService.Data;
using AuthService.Data.DTOs;
using AuthService.Extensions;
using AuthService.Interfaces;
using DataLayer.Contexts;

namespace AuthService.Services
{
    public class UserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        private readonly AcademyContext _context;

        public UserService(
            IPasswordHasher passwordHasher,
            IUserRepository userRepository,
            ITokenService tokenService,
            AcademyContext context)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _context = context;
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

        public async Task<TokenData> SignIn(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                // Exception
            }

            var passwordCheck = _passwordHasher.Verify(password, user.PasswordHash);

            if (!passwordCheck)
            {
                // TODO: Custom exception
                throw new Exception("Что то случилось");
            }

            var accesstoken = await _tokenService.Generate(user);
            var refreshToken = await _tokenService.GenerateRefreshToken();

            return new()
            {
                AccessToken = accesstoken,
                RefreshToken = refreshToken,
                RefreshTokenExpired = DateTime.UtcNow.AddDays(1),
            };
        }

        public async Task<TokenData> RefreshTokenAsync(TokenData data)
        {
            ArgumentNullException.ThrowIfNull(data);

            var principal = await _tokenService.GetPrincipalFromExpiredToken(data.AccessToken);

            var userId = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var user = await _context.Users.FindAsync(userId);

            if (user is null ||
                user.RefreshToken != data.RefreshToken ||
                user.RefreshTokenExpiryTime < DateTime.Now)
            {
                // TODO: Custom Exception
                throw new Exception($"From {nameof(RefreshTokenAsync)}");
            }

            var accessToken = await _tokenService.Generate(user);
            var refreshToken = await _tokenService.GenerateRefreshToken();
            var expiryTime = DateTime.UtcNow.AddDays(1);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiryTime;

            await _context.SaveChangesAsync();

            return new TokenData()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpired = expiryTime
            };
        }
    }
}
