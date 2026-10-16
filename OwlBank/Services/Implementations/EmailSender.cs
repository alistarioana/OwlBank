using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using OwlBank.Services.Interfaces;

namespace OwlBank.Services.Implementations
{
    public class SmtpOptions
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 587;
        public string User { get; set; } = "";
        public string Password { get; set; } = "";
        public string From { get; set; } = "";
    }

    [Dependency(typeof(IEmailSender))]

    public class EmailSender(IOptions<SmtpOptions> options) : IEmailSender
    {
        private readonly SmtpOptions _o = options.Value;

        public async Task SendAsync(string to, string subject, string html)
        {
            var msg = new MimeMessage();
            msg.From.Add(MailboxAddress.Parse(_o.From));
            msg.To.Add(MailboxAddress.Parse(to));
            msg.Subject = subject;
            msg.Body = new BodyBuilder { HtmlBody = html }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_o.Host, _o.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_o.User, _o.Password);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
    }
}
