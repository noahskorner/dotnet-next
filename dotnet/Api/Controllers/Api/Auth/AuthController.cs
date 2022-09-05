using Api.Constants;
using Services.Configuration;

namespace Api.Controllers.Api.Auth
{
    public class AuthController : ApiController
    {
        private readonly AppConfiguration _appConfig;

        public AuthController(AppConfiguration appConfig)
        {
            _appConfig = appConfig;
        }

        protected string RefreshToken
        {
            get
            {
                if (!Request.Cookies.ContainsKey(ApiConstants.TOKEN_COOKIE_KEY)) throw new RefreshTokenNotFoundException();

                return Request.Cookies.Single(x => x.Key == ApiConstants.TOKEN_COOKIE_KEY).Value;
            }
        }

        protected void SetRefreshTokenCookie(HttpResponse response, string refreshToken, DateTimeOffset refreshTokenExpiration)
        {
            var cookieOptions = new CookieOptions()
            {
                Domain = _appConfig.BackendDomain,
                Secure = true,
                Expires = refreshTokenExpiration
            };
            response.Cookies.Append(ApiConstants.TOKEN_COOKIE_KEY, refreshToken, cookieOptions);
        }
    }

    public class RefreshTokenNotFoundException : Exception
    {
        public RefreshTokenNotFoundException() : base("Refresh token was not found on this request.")
        {
        }
    }
}
