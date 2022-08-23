namespace Services.Configuration
{
    public class JwtConfiguration
    {
        public const string Jwt = "Jwt";
        public string EmailVerificationSecret { get; init; }
        public string AccessTokenSecret { get; init; }
        public string RefreshTokenSecret { get; init; }
    }
}
