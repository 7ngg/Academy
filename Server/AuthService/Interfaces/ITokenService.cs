using DataLayer.Models;
using System.Security.Claims;

namespace AuthService.Interfaces
{
    public interface ITokenService
    {
        Task<string> Generate(User user);
        string GenerateRandomToken();
        Task<ClaimsIdentity> GetPrincipalFromExpiredToken(string token);
    }
}
