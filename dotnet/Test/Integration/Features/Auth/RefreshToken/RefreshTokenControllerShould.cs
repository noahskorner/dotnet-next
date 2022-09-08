using Api.Constants;
using Api.Controllers.Api.Auth;
using Api.Controllers.Api.Auth.Login;
using Services.Configuration;
using Services.Features.Auth;
using Services.Services;
using System.Net.Http.Json;
using System.Security.Claims;
using Test.Extensions;

namespace Test.Integration.Features.Auth.RefreshToken
{
    public class RefreshTokenControllerShould : ControllerFixture
    {
        private const string BASE_URL = $"{ApiConstants.ROUTE_PREFIX}/auth";

        [Test]
        public async Task ReturnUnauthorizedWhenTokenNotFound()
        {
            // Arrange && Act && Assert
            await _sut.PutAsync(BASE_URL, null).AsUnauthorized<AuthResponse>();
        }


        [Test]
        public async Task ReturnUnauthorizedWhenTokenIsInvalid()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Put, BASE_URL);
            request.Headers.Add("Cookie", $"{ApiConstants.TOKEN_COOKIE_KEY}=invalidtoken;");

            // Act && Assert
            await _sut.SendAsync(request).AsUnauthorized<AuthResponse>();
        }

        [Test]
        public async Task ReturnUnauthorizedWhenUserIdClaimNotFound()
        {
            // Arrange
            var jwtConfig = GetDep<JwtConfiguration>();
            var jwtService = GetDep<IJwtService>();
            var generateTokenRequest = new GenerateTokenRequest(jwtConfig.RefreshTokenSecret, new List<Claim>(), DateTimeOffset.Now + jwtConfig.RefreshTokenExpiresIn);
            var token = jwtService.GenerateToken(generateTokenRequest);
            var request = new HttpRequestMessage(HttpMethod.Put, BASE_URL);
            request.Headers.Add("Cookie", $"{ApiConstants.TOKEN_COOKIE_KEY}={token};");

            // Act && Assert
            await _sut.SendAsync(request).AsUnauthorized<AuthResponse>();
        }


        [Test]
        public async Task ReturnUnauthorizedWhenUserNotFound()
        {
            // Arrange
            var authService = GetDep<IAuthService>();
            var (token, _) = authService.GenerateRefreshToken(1, "jon.snow@iceandfire.org");
            var request = new HttpRequestMessage(HttpMethod.Put, BASE_URL);
            request.Headers.Add("Cookie", $"{ApiConstants.TOKEN_COOKIE_KEY}={token};");

            // Act && Assert
            await _sut.SendAsync(request).AsUnauthorized<AuthResponse>();
        }

        [Test]
        public async Task ReturnOkWhenTokenIsValid()
        {
            // Arrange
            var request = await CreateAndLoginUser(HttpMethod.Put, BASE_URL);

            // Act && Assert
            await _sut.SendAsync(request).AsOk<AuthResponse>();
        }

        [Test]
        public async Task ReturnAccessTokenWhenTokenIsValid()
        {
            // Arrange
            var request = await CreateAndLoginUser(HttpMethod.Put, BASE_URL);

            // Act
            var result = await _sut.SendAsync(request).AsOk<AuthResponse>();

            // Assert
            Assert.That(result.Data.AccessToken, Is.Not.Null);
        }

        [Test]
        public async Task SetHttpOnlyRefreshTokenCookie()
        {
            // Arrange
            var request = await CreateAndLoginUser(HttpMethod.Put, BASE_URL);

            // Act
            var response = await _sut.SendAsync(request);

            // Assert
            var refreshTokenCookie = response.Headers
                .Single(header => header.Key == "Set-Cookie" && header.Value.Where(x => x.Contains(ApiConstants.TOKEN_COOKIE_KEY)).Count() == 1)
                .Value
                .Single();
            Assert.That(refreshTokenCookie, Is.Not.Null);
        }

        [Test]
        public async Task SetNewHttpOnlyRefreshTokenCookie()
        {
            // Arrange
            var request = await CreateAndLoginUser(HttpMethod.Put, BASE_URL);
            var originalRefreshTokenCookie = request.Headers
                .Single(header => header.Key == "Cookie" && header.Value.Where(x => x.Contains(ApiConstants.TOKEN_COOKIE_KEY)).Count() == 1)
                .Value
                .Single();

            // Act
            var response = await _sut.SendAsync(request);

            // Assert
            var actual = response.Headers
                .Single(header => header.Key == "Set-Cookie" && header.Value.Where(x => x.Contains(ApiConstants.TOKEN_COOKIE_KEY)).Count() == 1)
                .Value
                .Single();
            Assert.That(originalRefreshTokenCookie, Is.Not.EqualTo(actual));
        }

    }
}
