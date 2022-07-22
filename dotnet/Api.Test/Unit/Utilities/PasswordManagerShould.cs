using Api.Utilities;
using Api.Utilities.PasswordManager;

namespace Api.Test.Unit.Utilities
{
    public class PasswordManagerShould
    {
        private Random _random;
        private IPasswordManager _passwordManager;

        [SetUp]
        public void Setup()
        {
            _random = new Random();
            _passwordManager = new PasswordManager();
        }

        [Test]
        public void ReturnHashedPassword()
        {
            // Arrange 
            var password = _random.Next().ToString();

            // Act
            var result = _passwordManager.Hash(password);

            // Assert
            Assert.That(password, Is.Not.EqualTo(result));
        }

        [Test]
        public void ReturnDifferentHashedPasswordsForSameString()
        {
            // Arrange
            var password = _random.Next().ToString();

            // Act
            var result1 = _passwordManager.Hash(password);
            var result2 = _passwordManager.Hash(password);

            // Assert
            Assert.That(result1, Is.Not.EqualTo(result2));
        }

        [Test]
        public void ThrowInvalidPasswordExceptionWhenPasswordContainsPeriod()
        {
            // Arrange
            var password = $"{_random.Next()}.";

            // Act && Assert
            Assert.Throws<InvalidPasswordException>(() => _passwordManager.Hash(password));
        }

        [Test]
        public void ReturnTrueWhenPasswordIsCorrect()
        {
            // Arrange
            var password = _random.Next().ToString();
            var hashedPassword = _passwordManager.Hash(password);

            // Act
            var result = _passwordManager.Verify(password, hashedPassword);

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
            var result = _passwordManager.Verify(password, hashedPassword);

            // Assert
            Assert.False(result);
        }
    }
}