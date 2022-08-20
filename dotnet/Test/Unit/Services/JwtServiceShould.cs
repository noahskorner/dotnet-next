using Domain.Services;

namespace Test.Unit.Services
{
    public class JwtServiceShould
    {
        private const string _secretKey = "adcb5c6b680578bb7236661d291ef368c8c4c010551e01653d85e49d72cdae7dd3d70c8ca168b63931fece4dd59c4b5ee22647998df9b6f0409d254b2ac4321b";
        private readonly IJwtService _sut = new JwtService();

        [Test]
        public void ReturnAToken()
        {
            // Arrange && Act
            var token = _sut.GenerateToken(_secretKey);

            // Assert
            Assert.That(token, Is.Not.Null);
        }

        [Test]
        public void ReturnTrueWhenTokenIsValid()
        {
            // Arrange
            var token = _sut.GenerateToken(_secretKey);

            // Act
            var isValid = _sut.ValidateToken(token, _secretKey);

            // Assert
            Assert.That(isValid, Is.True);
        }


        [Test]
        public void ReturnFalseWhenTokenIsNotValid()
        {
            // Act
            var isValid = _sut.ValidateToken("Jon Snow", _secretKey);

            // Assert
            Assert.That(isValid, Is.False);
        }
    }
}
