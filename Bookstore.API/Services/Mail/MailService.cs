using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Bookstore.API.Services
{
    public class MailService : IMailService
    {
        //private static string _fromEmail = "coffeeshop2g1g@gmail.com";
        //private static string _senderPasswd = "gwgzlaleifibvfda";
        private readonly string _senderPasswd;
        private readonly string _fromEmail;
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
            _senderPasswd =  _config["EmailSettings:SenderPassword"];
            _fromEmail = _config["EmailSettings:FromEmail"];
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body, string filePath = "")
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sahara Bookstore", _fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var builder = new BodyBuilder { TextBody = body };
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                builder.Attachments.Add(filePath);
            }
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_fromEmail, _senderPasswd);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi mail Sahara: {ex.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}