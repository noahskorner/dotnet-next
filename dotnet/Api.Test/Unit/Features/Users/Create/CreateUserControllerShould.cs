using Api.Domain.User;
using Api.Features.User.Create;
using Api.Test.Extensions;


namespace Api.Test.Unit.Features.Users.Create
{
    public class CreateUserControllerShould : ControllerFixture
    {
        public const string BASE_URL = "/api/user";

        [Test]
        public async Task ReturnBadRequestWhenEmailIsInvalid()
        {
            // Arrange
            var request = new CreateUserRequest("", "123456aB$");

            // Act && Assert
            await _sut.AsBadRequest<UserDto>(BASE_URL, request);
        }

        [Test]
        public async Task ReturnEmailValidationError()
        {
            // Arrange
            var request = new CreateUserRequest("", "123456aB$");

            // Act
            var result = await _sut.AsBadRequest<UserDto>(BASE_URL, request);

            // Assert
            Assert.That(result.Errors.Any(x => x.Field == nameof(CreateUserCommand.Email)), Is.True);
        }

        [Test]
        [TestCase("")]
        [TestCase("1234aB$")]
        [TestCase("12345ab$.")]
        [TestCase("123456aB")]
        public async Task ReturnBadRequestWhenPasswordIsInvalid(string password)
        {
            // Arrange
            var request = new CreateUserRequest(_faker.Internet.Email(), password);

            // Act && Assert
            await _sut.AsBadRequest<UserDto>(BASE_URL, request);
        }

        [Test]
        [TestCase("")]
        [TestCase("1234aB$")]
        [TestCase("12345ab$.")]
        [TestCase("123456aB")]
        public async Task ReturnPasswordValidationError(string password)
        {
            // Arrange
            var request = new CreateUserRequest(_faker.Internet.Email(), password);

            // Act
            var result = await _sut.AsBadRequest<UserDto>(BASE_URL, request);

            // Assert
            Assert.That(result.Errors.Any(x => x.Field == nameof(CreateUserCommand.Password)), Is.True);
        }

        [Test]
        public async Task ReturnBadRequestWhenEmailAlreadyExists()
        {
            // Arrange
            var request = new CreateUserRequest(_faker.Internet.Email(), "123456aB$");

            // Act && Assert
            await _sut.AsBadRequest<UserDto>(BASE_URL, request);
        }

        [Test]
        public async Task ReturnCreated()
        {

        }

        [Test]
        public async Task ReturnUserId()
        {

        }
    }
}