using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using SmartStock.Infrastructure.Config;

namespace SmartStock.Infrastructure.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Inventory App", _settings.SmtpUser));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            message.Body = new TextPart("html") { Text = htmlContent };

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, false);
            await client.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
