using Api.Constants;
using Services.Features.Users;
using Test.Extensions;

namespace Test.Integration.Features.Users.Get
{
    public class GetUserControllerShould : ControllerFixture
    {
        private const string BASE_URL = $"{ApiConstants.ROUTE_PREFIX}/user";

        [Test]
        public async Task ReturnUnauthorizedWhenUserIsNotLoggedIn()
        {
            // Arrange && Act && Assert
            await _sut.GetAsync($"{BASE_URL}/1").AsUnauthorized<UserDto>();
        }

        [Test]
        public async Task ReturnForbiddenWhenRouteIdDoesNotMatchUserId()
        {
            // Arrange
            var (request, user) = await CreateAndLoginUser(HttpMethod.Get);
            request.RequestUri = new Uri($"{BASE_URL}/{user.Id + 1}", UriKind.Relative);

            // Act && Assert
            await _sut.SendAsync(request).AsForbidden<UserDto>();
        }

        [Test]
        public async Task ReturnOk()
        {
            // Arrange
            var (request, user) = await CreateAndLoginUser(HttpMethod.Get);
            request.RequestUri = new Uri($"{BASE_URL}/{user.Id}", UriKind.Relative);

            // Act && Assert
            await _sut.SendAsync(request).AsOk<UserDto>();
        }

        [Test]
        public async Task ReturnUserWithId()
        {
            // Arrange
            var (request, user) = await CreateAndLoginUser(HttpMethod.Get);
            request.RequestUri = new Uri($"{BASE_URL}/{user.Id}", UriKind.Relative);

            // Act
            var result = await _sut.SendAsync(request).AsOk<UserDto>();

            // Assert
            Assert.That(result.Data.Id, Is.EqualTo(user.Id));
        }
    }
}