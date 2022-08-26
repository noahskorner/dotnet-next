using Microsoft.IdentityModel.Tokens;
using Services.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services
{
    public interface IJwtService
    {
        string GenerateToken(GenerateTokenRequest request);
        bool ValidateToken(ValidateTokenRequest request);
    }
    public class GenerateTokenRequest
    {
        public string SecretKey { get; }
        public IEnumerable<Claim> Claims { get; }
        public DateTime? Expires { get; }
        public GenerateTokenRequest(string secretKey, IEnumerable<Claim> claims, DateTime? expires)
        {
            SecretKey = secretKey;
            Claims = claims;
            Expires = expires;
        }
    }

    public class ValidateTokenRequest
    {
        public string Token { get; }
        public string SecretKey { get; }
        public bool ValidateLifetime { get; }

        public ValidateTokenRequest(string token, string secretKey, bool validateLifetime)
        {
            Token = token;
            SecretKey = secretKey;
            ValidateLifetime = validateLifetime;
        }
    }


    public class JwtService : IJwtService
    {
        private readonly JwtConfiguration _jwtConfig;

        public JwtService(JwtConfiguration jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public string GenerateToken(GenerateTokenRequest request)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(request.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: request.Claims,
                notBefore: DateTime.UtcNow,
                expires: request.Expires,
                signingCredentials: credentials);

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(securityToken);
        }

        public bool ValidateToken(ValidateTokenRequest request)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters(request.SecretKey, request.ValidateLifetime);
                tokenHandler.ValidateToken(request.Token, validationParameters, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private TokenValidationParameters GetValidationParameters(string secretKey, bool validateLifetime)
        {
            return new TokenValidationParameters()
            {
                ValidAudience = _jwtConfig.Audience,
                ValidIssuer = _jwtConfig.Issuer,
                ValidateLifetime = validateLifetime,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };
        }
    }
}
