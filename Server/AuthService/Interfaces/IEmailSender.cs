namespace AuthService.Interfaces
{
    public interface IEmailSender
    {
        Task SendAsync(string recipient, string subject, string body);
    }
}
