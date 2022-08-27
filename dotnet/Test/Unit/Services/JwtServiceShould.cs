using Services.Configuration;
using Services.Services;
using System.Security.Claims;

namespace Test.Unit.Services
{
    public class JwtServiceShould
    {
        private const string _secretKey = "adcb5c6b680578bb7236661d291ef368c8c4c010551e01653d85e49d72cdae7dd3d70c8ca168b63931fece4dd59c4b5ee22647998df9b6f0409d254b2ac4321b";
        private readonly IJwtService _sut = new JwtService(new JwtConfiguration() { Issuer = "https://localhost:5000", Audience = "https://localhost:5000" }, new DateService());

        [Test]
        public void ReturnAToken()
        {
            // Arrange && Act
            var generateTokenRequest = new GenerateTokenRequest(_secretKey, new List<Claim>(), null);
            var token = _sut.GenerateToken(generateTokenRequest);

            // Assert
            Assert.That(token, Is.Not.Null);
        }

        [Test]
        public void ReturnTrueWhenTokenIsValid()
        {
            // Arrange
            var generateTokenRequest = new GenerateTokenRequest(_secretKey, new List<Claim>(), null);
            var token = _sut.GenerateToken(generateTokenRequest);
            var validateTokenRequest = new ValidateTokenRequest(token, _secretKey, false);

            // Act
            var isValid = _sut.ValidateToken(validateTokenRequest);

            // Assert
            Assert.That(isValid, Is.True);
        }


        [Test]
        public void ReturnFalseWhenTokenIsNotValid()
        {
            // Arrange
            var validateTokenRequest = new ValidateTokenRequest("Jon Snow", _secretKey, false);

            // Act
            var isValid = _sut.ValidateToken(validateTokenRequest);

            // Assert
            Assert.That(isValid, Is.False);
        }
    }
}
