using Api.Constants;
using Test.Extensions;

namespace Test.Integration.Features.Auth.Logout
{
    public class LogoutControllerShould : ControllerFixture
    {
        private const string BASE_URL = $"{ApiConstants.ROUTE_PREFIX}/auth";

        [Test]
        public async Task ReturnUnauthorizedWhenUserNotLoggedIn()
        {
            // Arrange && Act && Assert
            await _sut.DeleteAsync(BASE_URL).AsUnauthorized();
        }

        [Test]
        public async Task ReturnOkWhenUserIsLoggedIn()
        {
            // Arrange
            var request = await CreateAndLoginUser(HttpMethod.Delete, BASE_URL);

            // Act && Assert
            await _sut.SendAsync(request).AsOk();
        }
    }
}
