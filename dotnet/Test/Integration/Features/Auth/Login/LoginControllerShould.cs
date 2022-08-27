using Api.Constants;
using Api.Controllers.Api.Auth;
using Api.Controllers.Api.Auth.Login;
using System.Net.Http.Json;
using Test.Extensions;

namespace Test.Integration.Features.Auth.Login
{
    public class LoginControllerShould : ControllerFixture
    {
        private const string BASE_URL = $"{ApiConstants.ROUTE_PREFIX}/auth";

        [TestCase(null)]
        [TestCase("")]
        [TestCase("thisisnotanemail")]
        public async Task ReturnBadRequestWhenEmailNotValid(string email)
        {
            // Arrange
            var request = new LoginRequest(email, "123456aB$");

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<AuthResponse>();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("thisisnotanemail")]
        public async Task ReturnEmailValidationError(string email)
        {
            // Arrange
            var request = new LoginRequest(email, "123456aB$");

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<AuthResponse>();

            // Assert
            result.ShouldHaveValidationErrorsFor(nameof(LoginRequest.Email));
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task ReturnBadRequestWhenPasswordNotValid(string password)
        {
            // Arrange
            var request = new LoginRequest(_faker.Internet.Email(), password);

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<AuthResponse>();
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task ReturnPasswordValidationError(string password)
        {
            // Arrange
            var request = new LoginRequest(_faker.Internet.Email(), password);

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<AuthResponse>();

            // Assert
            result.ShouldHaveValidationErrorsFor(nameof(LoginRequest.Password));
        }

        [Test]
        public async Task ReturnUnauthorizedWhenUserWithEmailDoesNotExist()
        {
            // Arrange
            var request = new LoginRequest(_faker.Internet.Email(), _faker.Internet.Password());

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsUnauthorized<AuthResponse>();
        }

        [Test]
        public async Task ReturnInvalidEmailOrPasswordErrorWhenEmailDoesNotExist()
        {
            // Arrange
            var request = new LoginRequest(_faker.Internet.Email(), _faker.Internet.Password());

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsUnauthorized<AuthResponse>();

            // Assert
            result.ShouldHaveErrorsFor(Errors.LOGIN_INVALID_EMAIL_OR_PASSWORD);
        }

        [Test]
        public async Task ReturnUnauthorizedWhenUserPasswordNotValid()
        {
            // Arrange
            var user = await CreateUser();
            var request = new LoginRequest(user.Email, _faker.Internet.Password());

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsUnauthorized<AuthResponse>();
        }

        [Test]
        public async Task ReturnInvalidEmailOrPasswordErrorWhenPasswordNotValid()
        {
            // Arrange
            var user = await CreateUser();
            var request = new LoginRequest(user.Email, _faker.Internet.Password());

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsUnauthorized<AuthResponse>();

            // Assert
            result.ShouldHaveErrorsFor(Errors.LOGIN_INVALID_EMAIL_OR_PASSWORD);
        }

        [Test]
        public async Task ReturnForbiddenWhenEmailIsNotVerified()
        {
            // Arrange
            var password = "123456aB$";
            var user = await CreateUser(password: password, isEmailVerified: false);
            var request = new LoginRequest(user.Email, password);

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsForbidden<AuthResponse>();
        }

        [Test]
        public async Task ReturnEmailNotVerifiedErrorWhenEmailIsNotVerified()
        {
            // Arrange
            var password = "123456aB$";
            var user = await CreateUser(password: password, isEmailVerified: false);
            var request = new LoginRequest(user.Email, password);

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsForbidden<AuthResponse>();

            // Assert
            result.ShouldHaveErrorsFor(Errors.LOGIN_EMAIL_NOT_VERIFIED);
        }

        [Test]
        public async Task ReturnCreatedWhenEmailAndPasswordAreValid()
        {
            // Arrange
            var password = "123456aB$";
            var user = await CreateUser(password: password);
            var request = new LoginRequest(user.Email, password);

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsCreated<AuthResponse>();
        }

        [Test]
        public async Task ReturnAccessToken()
        {
            // Arrange
            var password = "123456aB$";
            var user = await CreateUser(password: password);
            var request = new LoginRequest(user.Email, password);

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsCreated<AuthResponse>();

            // Assert
            Assert.That(result.Data.AccessToken, Is.Not.Null);
        }

        [Test]
        public async Task SetHttpOnlyRefreshTokenCookie()
        {
            // Arrange
            var password = "123456aB$";
            var user = await CreateUser(password: password);
            var request = new LoginRequest(user.Email, password);

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request);

            // Assert
            var refreshTokenCookie = result.Headers
                .Single(header => header.Key == "Set-Cookie" && header.Value.Where(x => x.Contains(ApiConstants.TOKEN_COOKIE_KEY)).Count() == 1)
                .Value
                .Single();
            Assert.That(refreshTokenCookie, Is.Not.Null);
        }
    }
}
