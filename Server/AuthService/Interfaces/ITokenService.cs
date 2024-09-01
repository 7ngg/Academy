using DataLayer.Models;
using System.Security.Claims;

namespace AuthService.Interfaces
{
    public interface ITokenService
    {
        Task<string> Generate(User user);
        Task<string> GenerateRandomToken();
        Task<ClaimsIdentity> GetPrincipalFromExpiredToken(string token);
    }
}
