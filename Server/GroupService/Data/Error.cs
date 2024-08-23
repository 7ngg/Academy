namespace GroupService.Data
{
    public class Error
    {
        public bool IsError { get; init; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
