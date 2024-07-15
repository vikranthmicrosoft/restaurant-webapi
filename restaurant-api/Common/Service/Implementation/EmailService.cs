using restaurant_api.Common.Service.Interface;
using System.Net;
using System.Net.Mail;

namespace restaurant_api.Common.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string htmlBody, string textBody)
        {

            // SMTP server configuration
            string smtpAddress = _configuration.GetSection("SMTP:Address").Value;
            int portNumber = Convert.ToInt32(_configuration.GetSection("SMTP:Port").Value);
            bool enableSSL = true;
            string SenderAddress = _configuration.GetSection("SMTP:SenderAddress").Value;
            string password = _configuration.GetSection("SMTP:Password").Value;
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(SenderAddress);
                mail.To.Add(recipientEmail);
                mail.Subject = subject;
                mail.Body = htmlBody;
                mail.IsBodyHtml = true; // Can set to false if the email body is plain text

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(SenderAddress, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.UseDefaultCredentials = false;
                    try
                    {
                        await smtp.SendMailAsync(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }
    }
}
