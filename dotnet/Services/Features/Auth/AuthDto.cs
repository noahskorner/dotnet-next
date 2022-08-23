namespace Services.Features.Auth
{
    public class AuthDto
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }

        public AuthDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
