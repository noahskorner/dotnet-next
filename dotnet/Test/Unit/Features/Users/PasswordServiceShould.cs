using Services.Features.Users;

namespace Test.Unit.Features.Users
{
    public class PasswordServiceShould
    {
        private Random _random;
        private IPasswordService _sut;

        [SetUp]
        public void Setup()
        {
            _random = new Random();
            _sut = new PasswordService();
        }

        [Test]
        public void ReturnHashedPassword()
        {
            // Arrange 
            var password = _random.Next().ToString();

            // Act
            var result = _sut.Hash(password);

            // Assert
            Assert.That(password, Is.Not.EqualTo(result));
        }

        [Test]
        public void ReturnDifferentHashedPasswordsForSameString()
        {
            // Arrange
            var password = _random.Next().ToString();

            // Act
            var result1 = _sut.Hash(password);
            var result2 = _sut.Hash(password);

            // Assert
            Assert.That(result1, Is.Not.EqualTo(result2));
        }

        [Test]
        public void ThrowInvalidPasswordExceptionWhenPasswordContainsPeriod()
        {
            // Arrange
            var password = $"{_random.Next()}.";

            // Act && Assert
            Assert.Throws<InvalidPasswordException>(() => _sut.Hash(password));
        }

        [Test]
        public void ReturnTrueWhenPasswordIsCorrect()
        {
            // Arrange
            var password = _random.Next().ToString();
            var hashedPassword = _sut.Hash(password);

            // Act
            var result = _sut.Verify(password, hashedPassword);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void ReturnFalseWhenPasswordIsIncorrect()
        {
            // Arrange
            var password = _random.Next().ToString();
            var hashedPassword = _random.Next().ToString();

            // Act
            var result = _sut.Verify(password, hashedPassword);

            // Assert
            Assert.False(result);
        }
    }
}