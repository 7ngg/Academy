using AuthService.Data;
using AuthService.Interfaces;
using DataLayer.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _options;

        public TokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> Generate(User user)
        {
            Claim[] claims =
            [
                new("userId",
                user.Id.ToString()),
                new(ClaimTypes.Role,
                user.Role.ToString())
            ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        public string GenerateRandomToken()
        {
            var timestamp = DateTime.UtcNow.ToBinary().ToString();
            var uid = Guid.NewGuid().ToString();

            var token = $"{timestamp}{uid}";

            return token;
        }

        public async Task<ClaimsIdentity> GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_options.SecretKey)),
                RoleClaimType = ClaimTypes.Role
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
            
            if (!tokenValidationResult.IsValid)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return tokenValidationResult.ClaimsIdentity;
        }
    }
}
