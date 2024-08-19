namespace AuthService.Data
{
    public class TokenData
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime RefreshTokenExpired { get; set; }
    }
}
