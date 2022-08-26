namespace Services.Features.Auth
{
    public class AuthDto
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime RefreshTokenExpiration { get; }

        public AuthDto(string accessToken, string refreshToken, DateTime refreshTokenExpiration)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshTokenExpiration = refreshTokenExpiration;
        }
    }
}
