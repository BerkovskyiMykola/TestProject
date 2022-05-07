using MimeKit;
using MailKit.Net.Smtp;

namespace TestProject.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly string _serviceEmail = "fa3af6e8a3a1e8";
        private readonly string _serviceEmailPassword = "fabcf2955ab190";

        public async Task SendEmailAsync(string email, string subject, BodyBuilder builder)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Example", _serviceEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.mailtrap.io", 465, false);
            await client.AuthenticateAsync(_serviceEmail, password: _serviceEmailPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
