namespace Bookstore.API.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, string filePath = "");
    }
}
