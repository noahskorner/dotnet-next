namespace Api.Services.MailService
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(string to, string subject, string body);
    }
}
