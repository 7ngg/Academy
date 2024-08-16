using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.DTOs
{
    public record SignInDTO
    (
        [Required] string Username,
        [Required] string Password
    );
}
