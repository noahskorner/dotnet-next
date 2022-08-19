namespace Domain.Providers.MailProvider
{
    public interface IMailProvider
    {
        Task<bool> SendMailAsync(string to, string subject, string body);
    }
}
