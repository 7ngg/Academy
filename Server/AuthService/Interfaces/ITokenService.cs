using DataLayer.Models;

namespace AuthService.Interfaces
{
    public interface ITokenService
    {
        string Generate(User user);
    }
}
