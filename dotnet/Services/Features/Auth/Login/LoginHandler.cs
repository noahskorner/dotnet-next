using Data.Repositories.Users;
using MediatR;
using Services.Configuration;
using Services.Services;
using System.Security.Claims;

namespace Services.Features.Auth.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthDto>
    {
        private readonly IGetUserByEmail _getUserByEmail;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly JwtConfiguration _jwtConfig;

        public LoginHandler(
            IGetUserByEmail getUserByEmail,
            IPasswordService passwordService,
            IJwtService jwtService,
            JwtConfiguration jwtConfig)
        {
            _getUserByEmail = getUserByEmail;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _jwtConfig = jwtConfig;
        }

        public async Task<AuthDto> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _getUserByEmail.Execute(command.Email);
            if (user == null) throw new LoginUserNotFoundException(command.Email);

            var isValidPassword = _passwordService.Verify(command.Password, user.Password);
            if (!isValidPassword) throw new LoginInvalidPasswordException(command.Email);

            var accessToken = GetAccessToken(user.Id, user.Email);
            var (refreshToken, refreshTokenExpiration) = GetRefreshToken(user.Id, user.Email);

            return new AuthDto(
                accessToken,
                refreshToken,
                refreshTokenExpiration);
        }

        private string GetAccessToken(long userId, string email)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
            };
            var expiresIn = DateTime.UtcNow + _jwtConfig.AccessTokenExpiresIn;
            var request = new GenerateTokenRequest(_jwtConfig.AccessTokenSecret, claims, expiresIn);

            return _jwtService.GenerateToken(request);
        }

        private (string, DateTime) GetRefreshToken(long userId, string email)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
            };
            var expiresIn = DateTime.UtcNow + _jwtConfig.RefreshTokenExpiresIn;
            var request = new GenerateTokenRequest(_jwtConfig.RefreshTokenSecret, claims, expiresIn);

            return (_jwtService.GenerateToken(request), expiresIn);
        }
    }

    public class LoginUserNotFoundException : Exception
    {
        public string Email { get; }

        public LoginUserNotFoundException(string email) : base($"User with email {email} does not exist.")
        {
            Email = email;
        }
    }

    public class LoginInvalidPasswordException : Exception
    {
        public string Email { get; }

        public LoginInvalidPasswordException(string email) : base($"Invalid password for user {email}.")
        {
            Email = email;
        }
    }
}
