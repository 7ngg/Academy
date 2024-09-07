namespace AuthService.Data
{
    public class Error
    {
        public required int Code { get; init; }
        public required string Message { get; init; }
        public required bool IsError { get; init; }
    }
}
