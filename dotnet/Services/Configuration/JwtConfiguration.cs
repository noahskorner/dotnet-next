namespace Services.Configuration
{
    public class JwtConfiguration
    {
        public const string Jwt = "Jwt";
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string EmailVerificationSecret { get; init; }
        public string AccessTokenSecret { get; init; }
        public TimeSpan AccessTokenExpiresIn { get; init; }
        public string RefreshTokenSecret { get; init; }
        public TimeSpan RefreshTokenExpiresIn { get; init; }
    }
}
