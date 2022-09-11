using Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;
using Services.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Services.Features.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfiguration _jwtConfig;
        private readonly IDateService _dateService;

        public JwtService(
            JwtConfiguration jwtConfig,
            IDateService dateService)
        {
            _jwtConfig = jwtConfig;
            _dateService = dateService;
        }

        public string GenerateToken(GenerateTokenRequest request)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(request.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: request.Claims,
                notBefore: _dateService.Now().DateTime,
                expires: request.Expires?.DateTime ?? null,
                signingCredentials: credentials);

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(securityToken);
        }

        public JwtSecurityToken? ValidateToken(ValidateTokenRequest request)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters(request.SecretKey, request.ValidateLifetime);
                tokenHandler.ValidateToken(request.Token, validationParameters, out SecurityToken validatedToken);

                return validatedToken as JwtSecurityToken;
            }
            catch
            {
                return null;
            }
        }

        private TokenValidationParameters GetValidationParameters(string secretKey, bool validateLifetime)
        {
            return new TokenValidationParameters()
            {
                ValidAudience = _jwtConfig.Audience,
                ValidIssuer = _jwtConfig.Issuer,
                ClockSkew = new TimeSpan(0),
                ValidateLifetime = validateLifetime,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };
        }
    }
}
