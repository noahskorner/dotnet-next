using Domain.Models.Users;

namespace Services.Features.Auth
{
    public interface IAuthService
    {
        string GenerateAccessToken(long userId, string email, IEnumerable<Role> roles);
        (string, DateTimeOffset) GenerateRefreshToken(long userId, string email);
    }
}
