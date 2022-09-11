using Data.Repositories.Users;
using MediatR;
using Services.Configuration;
using System.Security.Claims;

namespace Services.Features.Auth.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthDto>
    {
        private readonly IJwtService _jwtService;
        private readonly JwtConfiguration _jwtConfig;
        private readonly IGetUserById _getUserById;
        private readonly IAuthService _authService;

        public RefreshTokenHandler(
            IJwtService jwtService,
            JwtConfiguration jwtConfig,
            IGetUserById getUserById,
            IAuthService authService)
        {
            _jwtService = jwtService;
            _jwtConfig = jwtConfig;
            _getUserById = getUserById;
            _authService = authService;
        }

        public async Task<AuthDto> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var userId = GetUserIdFromToken(command.RefreshToken);
            var user = await _getUserById.Execute(userId);
            if (user == null) throw new RefreshTokenUserNotFoundException(userId);

            var accessToken = _authService.GenerateAccessToken(user.Id, user.Email, user.Roles);
            var (refreshToken, refreshTokenExpiration) = _authService.GenerateRefreshToken(user.Id, user.Email);

            return new AuthDto(accessToken, refreshToken, refreshTokenExpiration);
        }

        private long GetUserIdFromToken(string refreshToken)
        {
            var validateTokenRequest = new ValidateTokenRequest(refreshToken, _jwtConfig.RefreshTokenSecret, true);
            var validatedToken = _jwtService.ValidateToken(validateTokenRequest);
            if (validatedToken == null) throw new RefreshTokenInvalidTokenException(refreshToken);

            var userIdClaim = validatedToken.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) throw new RefreshTokenInvalidTokenException(refreshToken);

            if (!long.TryParse(userIdClaim.Value, out long userId)) throw new RefreshTokenInvalidTokenException(refreshToken);

            return userId;
        }
    }

    public class RefreshTokenInvalidTokenException : Exception
    {
        public string Token { get; }
        public RefreshTokenInvalidTokenException(string token) : base("The provided refresh token was invalid")
        {
            Token = token;
        }
    }

    public class RefreshTokenUserNotFoundException : Exception
    {
        public long UserId { get; }

        public RefreshTokenUserNotFoundException(long userId) : base($"User {userId} was not found.")
        {
            UserId = userId;
        }
    }
}
