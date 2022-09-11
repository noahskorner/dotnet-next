using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services.Features.Auth
{
    public interface IJwtService
    {
        string GenerateToken(GenerateTokenRequest request);
        JwtSecurityToken? ValidateToken(ValidateTokenRequest request);
    }
    public class GenerateTokenRequest
    {
        public string SecretKey { get; }
        public IEnumerable<Claim> Claims { get; }
        public DateTimeOffset? Expires { get; }
        public GenerateTokenRequest(string secretKey, IEnumerable<Claim> claims, DateTimeOffset? expires)
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

}
