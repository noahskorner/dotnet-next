namespace Api.Configuration
{
    public class JwtConfiguration
    {
        public const string Jwt = "Jwt";
        public string EmailVerificationSecret { get; set; }
    }
}
