namespace Domain.Configuration
{
    public class SmtpConfiguration
    {
        public const string Smtp = "Smtp";
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }
}
