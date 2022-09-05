namespace Api.Controllers.Api.Auth
{
    public class AuthResponse
    {
        public string AccessToken { get; }

        public AuthResponse(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
