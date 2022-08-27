using Api.Constants;
using Services.Features.Users;
using Test.Extensions;

namespace Test.Integration.Features.Users.VerifyEmail
{
    public class VerifyEmailControllerShould : ControllerFixture
    {
        private const string BASE_URL = $"{ApiConstants.ROUTE_PREFIX}/user";

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
            var user = await CreateUser();
            var url = GetUrl(user.Id, "token");

            // Act && Assert
            await _sut.PutAsync(url, null).AsUnauthorized<UserDto>();
        }

        [Test]
        public async Task ReturnOkWhenTokenIsValid()
        {
            // Arrange
            var user = await CreateUser();
            var userEntity = await _context.User.FindAsync(user.Id);
            var verificationToken = userEntity?.EmailVerificationToken ?? throw new Exception();
            var url = GetUrl(user.Id, verificationToken);

            // Act && Assert
            await _sut.PutAsync(url, null).AsOk<UserDto>();
        }

        [Test]
        public async Task ReturnEmailWhenTokenIsValid()
        {
            // Arrange
            var user = await CreateUser();
            var userEntity = await _context.User.FindAsync(user.Id);
            var verificationToken = userEntity?.EmailVerificationToken ?? throw new Exception();
            var url = GetUrl(user.Id, verificationToken);

            // Act
            var result = await _sut.PutAsync(url, null).AsOk<UserDto>();

            // Assert
            Assert.That(result.Data.Email, Is.EqualTo(user.Email));
        }

        private string GetUrl(long userId, string token)
        {
            return $"{BASE_URL}/{userId}/verify-email/{token}";
        }

    }
}
