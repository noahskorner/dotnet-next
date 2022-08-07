using Api.Configuration;
using System.Net;
using System.Net.Mail;

namespace Api.Services.MailProvider
{
    public class MailProvider : IMailProvider
    {
        private readonly SmtpClient _client;
        private readonly SmtpConfiguration _config;

        public MailProvider(SmtpConfiguration config)
        {
            _config = config;
            _client = new SmtpClient()
            {
                Host = _config.Host,
                Port = _config.Port,
                Credentials = new NetworkCredential(_config.User, _config.Password),
                EnableSsl = _config.EnableSsl,
            };
        }

        public async Task<bool> SendMailAsync(string to, string subject, string body)
        {
            try
            {
                var toAddress = new MailAddress(to);
                var fromAddress = new MailAddress(_config.User);
                var mailMessage = new MailMessage(fromAddress, toAddress)
                {
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    Subject = subject,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    Body = body
                };

                await _client.SendMailAsync(mailMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
