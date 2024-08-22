namespace FacultyService.Data
{
    public class Error(int code, string message, bool isError = true)
    {
        public bool IsError { get; init; } = isError;
        public int Code { get; init; } = code;
        public string Message { get; init; } = message;
    }
}
