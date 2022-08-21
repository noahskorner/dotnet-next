using Api.Controllers.Api.Users.Create;
using Services.Features.Users;
using System.Net.Http.Json;
using Test.Extensions;

namespace Test.Integration.Features.Users.VerifyEmail
{
    public class VerifyEmailControllerShould : ControllerFixture
    {
        private const string BASE_URL = "api/user";

        [Test]
        public async Task ReturnNotFoundWhenUserDoesNotExist()
        {
            // Arrange
            var url = GetUrl(1, "token");

            // Act && Assert
            await _sut.PutAsync(url, null).AsNotFound<UserDto>();
        }

        [Test]
        public async Task ReturnUnauthorizedWhenTokenNotValid()
        {
            // Arrange
            var createUserRequest = new CreateUserRequest(_faker.Internet.Email(), "123456aB$");
            var createUserResult = await _sut.PostAsJsonAsync(BASE_URL, createUserRequest).AsCreated<UserDto>();
            var userEntity = await _context.User.FindAsync(createUserResult.Data.Id);
            var url = GetUrl(userEntity?.Id ?? 0, "token");

            // Act && Assert
            await _sut.PutAsync(url, null).AsUnauthorized<UserDto>();
        }

        [Test]
        public async Task ReturnOkWhenTokenIsValid()
        {
            // Arrange
            var createUserRequest = new CreateUserRequest(_faker.Internet.Email(), "123456aB$");
            var createUserResult = await _sut.PostAsJsonAsync(BASE_URL, createUserRequest).AsCreated<UserDto>();
            var userEntity = await _context.User.FindAsync(createUserResult.Data.Id);
            var url = GetUrl(1, userEntity?.EmailVerificationToken ?? "");

            // Act && Assert
            await _sut.PutAsync(url, null).AsOk<UserDto>();
        }

        [Test]
        public async Task ReturnEmailWhenTokenIsValid()
        {
            // Arrange
            var email = _faker.Internet.Email();
            var createUserRequest = new CreateUserRequest(email, "123456aB$");
            var createUserResult = await _sut.PostAsJsonAsync(BASE_URL, createUserRequest).AsCreated<UserDto>();
            var userEntity = await _context.User.FindAsync(createUserResult.Data.Id);
            var url = GetUrl(1, userEntity?.EmailVerificationToken ?? "");

            // Act
            var result = await _sut.PutAsync(url, null).AsOk<UserDto>();

            // Assert
            Assert.That(result.Data.Email, Is.EqualTo(email));
        }

        private string GetUrl(long userId, string token)
        {
            return $"{BASE_URL}/{userId}/verify-email/{token}";
        }

    }
}
