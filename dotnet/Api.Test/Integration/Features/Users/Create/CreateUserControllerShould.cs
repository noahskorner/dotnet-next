using Api.Controllers.Api.User.Create;
using Api.Test.Extensions;
using Data.Entities.User;
using Domain.Features.User;
using Domain.Features.User.Create;
using System.Net.Http.Json;

namespace Api.Test.Integration.Features.Users.Create
{
    public class CreateUserControllerShould : ControllerFixture
    {
        private const string BASE_URL = "api/user";

        [Test]
        public async Task ReturnBadRequestWhenEmailIsInvalid()
        {
            // Arrange
            var request = new CreateUserRequest("", "123456aB$");

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<UserDto>();
        }

        [Test]
        public async Task ReturnEmailValidationError()
        {
            // Arrange
            var request = new CreateUserRequest("", "123456aB$");

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<UserDto>();

            // Assert
            Assert.That(result.Errors.Any(x => x.Field == nameof(CreateUserCommand.Email)), Is.True);
        }

        [Test]
        [TestCase("")]
        [TestCase("1234aB$")]
        [TestCase("123456aB")]
        [TestCase("12345aB$.")]
        public async Task ReturnBadRequestWhenPasswordIsInvalid(string password)
        {
            // Arrange
            var request = new CreateUserRequest(_faker.Internet.Email(), password);

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<UserDto>();
        }

        [Test]
        [TestCase("")]
        [TestCase("1234aB$")]
        [TestCase("123456aB")]
        [TestCase("12345aB$.")]
        public async Task ReturnPasswordValidationError(string password)
        {
            // Arrange
            var request = new CreateUserRequest(_faker.Internet.Email(), password);

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<UserDto>();

            // Assert
            Assert.That(result.Errors.Any(x => x.Field == nameof(CreateUserCommand.Password)), Is.True);
        }

        [Test]
        public async Task ReturnBadRequestWhenEmailAlreadyExists()
        {
            // Arrange
            var email = _faker.Internet.Email();
            await _context.User.AddAsync(new UserEntity(email, "123456aB$", ""));
            await _context.SaveChangesAsync();

            var request = new CreateUserRequest(email, "123456aB$");

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsBadRequest<UserDto>();
        }

        [Test]
        public async Task ReturnCreated()
        {
            // Arrange
            var email = _faker.Internet.Email();
            var request = new CreateUserRequest(email, "123456aB$");

            // Act && Assert
            await _sut.PostAsJsonAsync(BASE_URL, request).AsCreated<UserDto>();
        }

        [Test]
        public async Task ReturnUserId()
        {
            // Arrange
            var email = _faker.Internet.Email();
            var request = new CreateUserRequest(email, "123456aB$");

            // Act
            var result = await _sut.PostAsJsonAsync(BASE_URL, request).AsCreated<UserDto>();

            // Assert
            Assert.That(result.Data.Id, Is.AtLeast(1));
        }
    }
}