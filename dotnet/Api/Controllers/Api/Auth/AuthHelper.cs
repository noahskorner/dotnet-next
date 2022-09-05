using Api.Constants;
using Services.Configuration;

namespace Api.Controllers.Api.Auth
{
    public interface IAuthHelper
    {
        string GetRefreshToken(HttpRequest request);
        void SetRefreshTokenCookie(HttpResponse response, string refreshToken, DateTimeOffset refreshTokenExpiration);
    }

    public class AuthHelper : IAuthHelper
    {
        private readonly AppConfiguration _appConfig;

        public AuthHelper(AppConfiguration appConfig)
        {
            _appConfig = appConfig;
        }

        public string GetRefreshToken(HttpRequest request)
        {
            if (!request.Cookies.ContainsKey(ApiConstants.TOKEN_COOKIE_KEY)) throw new RefreshTokenNotFoundException();

            return request.Cookies.Single(x => x.Key == ApiConstants.TOKEN_COOKIE_KEY).Value;
        }

        public void SetRefreshTokenCookie(HttpResponse response, string refreshToken, DateTimeOffset refreshTokenExpiration)
        {
            var cookieOptions = GetTokenCookieOptions(refreshTokenExpiration);
            response.Cookies.Append(ApiConstants.TOKEN_COOKIE_KEY, refreshToken, cookieOptions);
        }

        private CookieOptions GetTokenCookieOptions(DateTimeOffset refreshTokenExpiration) =>
            new CookieOptions() { Domain = _appConfig.BackendDomain, Secure = true, Expires = refreshTokenExpiration };
    }

    public class RefreshTokenNotFoundException : Exception
    {
        public RefreshTokenNotFoundException() : base("Refresh token was not found on this request.")
        {
        }
    }

}
