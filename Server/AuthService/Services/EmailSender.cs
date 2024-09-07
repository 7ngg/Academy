using AuthService.Interfaces;
using System.Net;
using System.Net.Mail;

namespace AuthService.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        private string? _originEmail;
        private string? _password;
        private string? _smtpAdress;
        private int _port;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            Configure();
        }

        private void Configure()
        {
            _originEmail = _configuration.GetSection("smtp")["originEmail"];
            _password = _configuration.GetSection("smtp")["password"];
            _smtpAdress = _configuration.GetSection("smtp")["adress"];
            var portString = _configuration.GetSection("smtp")["port"];

            var isPortParsed = int.TryParse(portString, out _port);

            if (
                _originEmail is null ||
                _password is null ||
                _smtpAdress is null ||
                _smtpAdress is null ||
                !isPortParsed)
            {
                throw new Exception("Smtp configuration missing");
            }
        }

        public async Task SendAsync(string recipient, string subject, string body)
        {
            var client = new SmtpClient(_smtpAdress, _port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_originEmail, _password)
            };

            var message = new MailMessage()
            {
                From = new(_originEmail!),
                Subject = subject, 
                Body = body,
            };
            message.CC.Add(new(recipient));

            await client.SendMailAsync(message);
        }
    }
}
