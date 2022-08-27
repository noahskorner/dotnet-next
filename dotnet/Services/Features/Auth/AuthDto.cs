namespace Services.Features.Auth
{
    public class AuthDto
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTimeOffset RefreshTokenExpiration { get; }

        public AuthDto(string accessToken, string refreshToken, DateTimeOffset refreshTokenExpiration)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshTokenExpiration = refreshTokenExpiration;
        }
    }
}
