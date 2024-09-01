using AuthService.Data;
using AuthService.Data.DTOs;
using AuthService.Extensions;
using AuthService.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace AuthService.Services
{
    [ApiController]
    public class UserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;

        public UserService(
            IPasswordHasher passwordHasher,
            IUserRepository userRepository,
            ITokenService tokenService,
            IConfiguration configuration,
            ICacheService cacheService)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _configuration = configuration;
            _cacheService = cacheService;
        }

        public async Task SignUp(SignUpDTO request)
        {
            string passwordHash = _passwordHasher.Generate(request.Password);

            var user = UserFactory.Create(
                Guid.NewGuid(),
                request.Username, passwordHash,
                request.Email, request.Name, request.Surname);

            await _userRepository.AddAsync(user);
            await SendVerification(user);
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
            var refreshToken = await _tokenService.GenerateRandomToken();

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
            var user = _userRepository.Filter(u => u.Id.ToString() == userId).First();

            if (user is null ||
                user.RefreshToken != data.RefreshToken ||
                user.RefreshTokenExpiryTime < DateTime.Now)
            {
                // TODO: Custom Exception
                throw new Exception($"From {nameof(RefreshTokenAsync)}");
            }

            var accessToken = await _tokenService.Generate(user);
            var refreshToken = await _tokenService.GenerateRandomToken();
            var expiryTime = DateTime.UtcNow.AddDays(1);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiryTime;

            await _userRepository.SaveAsync();

            return new TokenData()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpired = expiryTime
            };
        }

        private async Task SendVerification(User user)
        {
            string? originEmail = _configuration.GetSection("smtp").GetSection("originEmail").Value;
            string? password = _configuration.GetSection("smtp").GetSection("password").Value;
            string? smtpAdress = _configuration.GetSection("smtp").GetSection("adress").Value;
            int smtpPort = Convert.ToInt32(_configuration.GetSection("smtp").GetSection("port").Value);

            var verificationToken = await _tokenService.GenerateRandomToken();

            string subject = "Academy account confirmation";
            string body = $"https://localhost:7171/verify?token={verificationToken}";

            await _cacheService.Set<string>(verificationToken, user.Id.ToString(), DateTime.UtcNow.AddMinutes(3));

            var smtp = new SmtpClient(smtpAdress, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(originEmail, password)
            };

            await smtp.SendMailAsync(originEmail, user.Email, subject, body);
        }
    }
}
