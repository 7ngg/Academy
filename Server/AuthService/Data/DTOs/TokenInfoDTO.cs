namespace AuthService.Data.DTOs
{
    public class TokenInfoDTO
    {
        public required string AccessToken { get; init; }
        public required string RefreshToken { get; init; }
    }
}
