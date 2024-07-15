namespace restaurant_api.Common.Service.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string recipientEmail, string subject, string htmlBody, string textBody);
    }
}
