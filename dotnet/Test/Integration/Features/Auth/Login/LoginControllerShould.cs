using Api.Controllers.Api.Auth.Login;
using Api.Controllers.Api.Users.Create;
using Domain.Constants;
using Services.Features.Auth;
using Services.Features.Users;
using System.Net.Http.Json;
using Test.Extensions;

namespace Test.Integration.Features.Auth.Login
{
    public class LoginControllerShould : ControllerFixture
    {
        private const string BASE_URL = "v1/auth";

        [TestCase(null)]
        [TestCase("")]
        [TestCase("thisisnotanemail")]
        public async Task ReturnBadRequestWhenEmailNotValid(string email)
        {
            // Arrange
            var request = new LoginRequest(email, "123456aB$");

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<LoginResponse>();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("thisisnotanemail")]
        public async Task ReturnEmailValidationError(string email)
        {
            // Arrange
            var request = new LoginRequest(email, "123456aB$");

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<LoginResponse>();

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
            await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<LoginResponse>();
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task ReturnPasswordValidationError(string password)
        {
            // Arrange
            var request = new LoginRequest(_faker.Internet.Email(), password);

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<LoginResponse>();

            // Assert
            result.ShouldHaveValidationErrorsFor(nameof(LoginRequest.Password));
        }

        [Test]
        public async Task ReturnUnauthorizedWhenUserWithEmailDoesNotExist()
        {
            // Arrange
            var request = new LoginRequest(_faker.Internet.Email(), _faker.Internet.Password());

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsUnauthorized<LoginResponse>();
        }

        [Test]
        public async Task ReturnInvalidEmailOrPasswordErrorWhenEmailDoesNotExist()
        {
            // Arrange
            var request = new LoginRequest(_faker.Internet.Email(), _faker.Internet.Password());

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsUnauthorized<LoginResponse>();

            // Assert
            result.ShouldHaveErrorsFor(nameof(Errors.LOGIN_USER_INVALID_EMAIL_OR_PASSWORD));
        }

        [Test]
        public async Task ReturnUnauthorizedWhenUserPasswordNotValid()
        {
            // Arrange
            var user = await CreateUser();
            var request = new LoginRequest(user.Email, _faker.Internet.Password());

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsUnauthorized<LoginResponse>();
        }

        [Test]
        public async Task ReturnInvalidEmailOrPasswordErrorWhenPasswordNotValid()
        {
            // Arrange
            var user = await CreateUser();
            var request = new LoginRequest(user.Email, _faker.Internet.Password());

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsUnauthorized<LoginResponse>();

            // Assert
            result.ShouldHaveErrorsFor(nameof(Errors.LOGIN_USER_INVALID_EMAIL_OR_PASSWORD));
        }

        [Test]
        public async Task ReturnCreatedWhenEmailAndPasswordAreValid()
        {
            // Arrange
            var password = "123456aB$";
            var user = await CreateUser(password: password);
            var request = new LoginRequest(user.Email, password);

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsCreated<LoginResponse>();
        }

        [Test]
        public async Task ReturnAccessToken()
        {
            // Arrange
            var password = "123456aB$";
            var user = await CreateUser(password: password);
            var request = new LoginRequest(user.Email, password);

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsCreated<LoginResponse>();

            // Assert
            Assert.That(result.Data.AccessToken, Is.Not.Null);
        }

        [Test]
        public async Task SetHttpOnlyRefreshTokenCookie()
        {
            // Arrange
            var password = "123456aB$";
            var createUserRequest = new CreateUserRequest(_faker.Internet.Email(), password);
            var createUserResult = await _sut.PostAsJsonAsync("v1/user", createUserRequest).AsCreated<UserDto>();
            var user = createUserResult.Data;
            var request = new LoginRequest(user.Email, password);

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request);

            // TODO
        }
    }
}
