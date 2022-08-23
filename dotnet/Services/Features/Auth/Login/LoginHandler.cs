using Data.Repositories.Users;
using MediatR;
using Services.Configuration;
using Services.Services;

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

            var accessToken = _jwtService.GenerateToken(_jwtConfig.AccessTokenSecret);
            var refreshToken = _jwtService.GenerateToken(_jwtConfig.RefreshTokenSecret);

            return new AuthDto(accessToken, refreshToken);
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
