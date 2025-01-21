using System.Net.Mail;
using System.Net;
using KoperasiTenteraApi.Interfaces;

namespace KoperasiTenteraApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;

        public EmailService(IConfiguration configuration)
        {
            _smtpHost = configuration["SMTPEmailSettings:SmtpHost"] ?? "";
            _smtpPort = int.Parse(configuration["SMTPEmailSettings:SmtpPort"] ?? "");
            _smtpUser = configuration["SMTPEmailSettings:SmtpUser"] ?? "";
            _smtpPassword = configuration["SMTPEmailSettings:SmtpPassword"] ?? "";
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpUser),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }
    }
}
