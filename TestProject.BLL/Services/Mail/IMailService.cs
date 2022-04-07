using MimeKit;

namespace TestProject.Services.Mail
{
    public interface IMailService
    {
        public Task SendEmailAsync(string email, string subject, BodyBuilder builder);
    }
}
