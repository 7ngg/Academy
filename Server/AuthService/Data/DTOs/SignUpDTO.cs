using DataLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.DTOs
{
    public record SignUpDTO
    (
        [Required] string Username,
        [Required] string Password,
        [Required] string Email,
        [Required] string Name,
        [Required] string Surname
    );
}
