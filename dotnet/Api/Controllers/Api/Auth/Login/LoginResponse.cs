namespace Api.Controllers.Api.Auth.Login
{
    public class LoginResponse
    {
        public string AccessToken { get; }

        public LoginResponse(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
