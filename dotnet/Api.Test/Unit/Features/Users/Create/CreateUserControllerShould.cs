using Api.Domain.User;
using Api.Features.User.Create;
using Api.Models;
using Api.Test.Extensions;
using System.Net.Http.Json;

namespace Api.Test.Unit.Features.Users.Create
{
    public class CreateUserControllerShould : ControllerFixture
    {
        [Test]
        public async Task ReturnBadRequestWhenEmailIsInvalid()
        {
            // Arrange
            var request = new CreateUserRequest("", "");

            // Act && Assert
            await _sut.AsBadRequest<UserDto>("/api/user", request);
        }
    }

}