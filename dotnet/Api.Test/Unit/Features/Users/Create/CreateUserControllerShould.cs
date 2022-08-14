using Api.Domain.User;
using Api.Features.User.Create;
using Api.Test.Extensions;

namespace Api.Test.Unit.Features.Users.Create
{
    public class CreateUserControllerShould : ControllerFixture<CreateUserController>
    {
        [OneTimeSetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task ReturnBadRequestWhenEmailIsInvalid()
        {
            // Arrange
            var request = new CreateUserRequest("", "123456aB$");

            // Act && Assert
            await _sut.Create(request).AsBadRequest<UserDto>();
        }

        [Test]
        public async Task ReturnEmailValidationError()
        {
            // Arrange
            var request = new CreateUserRequest("", "123456aB$");

            // Act
            var result = await _sut.Create(request).AsBadRequest<UserDto>();

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
            await _sut.Create(request).AsBadRequest<UserDto>();
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
            var result = await _sut.Create(request).AsBadRequest<UserDto>();

            // Assert
            Assert.That(result.Errors.Any(x => x.Field == nameof(CreateUserCommand.Password)), Is.True);
        }

        [Test]
        public async Task ReturnBadRequestWhenEmailAlreadyExists()
        {
            // Arrange
            var request = new CreateUserRequest(_faker.Internet.Email(), "123456aB$");

            // Act && Assert
            await _sut.Create(request).AsBadRequest<UserDto>();
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