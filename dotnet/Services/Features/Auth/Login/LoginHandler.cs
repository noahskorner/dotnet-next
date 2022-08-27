using Data.Repositories.Users;
using MediatR;
using Services.Services;

namespace Services.Features.Auth.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthDto>
    {
        private readonly IGetUserByEmail _getUserByEmail;
        private readonly IPasswordService _passwordService;
        private readonly IAuthService _authService;

        public LoginHandler(
            IGetUserByEmail getUserByEmail,
            IPasswordService passwordService,
            IAuthService authService)
        {
            _getUserByEmail = getUserByEmail;
            _passwordService = passwordService;
            _authService = authService;
        }

        public async Task<AuthDto> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _getUserByEmail.Execute(command.Email);
            if (user == null) throw new LoginUserNotFoundException(command.Email);
            if (!user.IsEmailVerified) throw new LoginUserEmailNotVerifiedException(command.Email);


            var isValidPassword = _passwordService.Verify(command.Password, user.Password);
            if (!isValidPassword) throw new LoginInvalidPasswordException(command.Email);

            var accessToken = _authService.GenerateAccessToken(user.Id, user.Email);
            var (refreshToken, refreshTokenExpiration) = _authService.GenerateRefreshToken(user.Id, user.Email);

            return new AuthDto(
                accessToken,
                refreshToken,
                refreshTokenExpiration);
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

    public class LoginUserEmailNotVerifiedException : Exception
    {
        public string Email { get; }

        public LoginUserEmailNotVerifiedException(string email) : base($"{email} is not a verified email address.")
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
