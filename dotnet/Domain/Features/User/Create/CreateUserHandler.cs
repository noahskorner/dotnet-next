using Data.Entities.User;
using Domain.Configuration;
using Domain.Providers.MailProvider;
using Domain.Services;
using MediatR;

namespace Domain.Features.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly JwtConfiguration _jwtConfig;
        private readonly IPasswordService _passwordService;
        private readonly IMailProvider _mailProvider;
        private readonly IJwtService _jwtService;
        private readonly IGetUserByEmail _getUserByEmail;
        private readonly ICreateUser _createUser;

        public CreateUserCommandHandler(
            IPasswordService passwordService,
            IMailProvider mailProvider,
            JwtConfiguration jwtConfig,
            IJwtService jwtService,
            IGetUserByEmail getUserByEmail,
            ICreateUser createUser)
        {
            _passwordService = passwordService;
            _mailProvider = mailProvider;
            _jwtConfig = jwtConfig;
            _jwtService = jwtService;
            _getUserByEmail = getUserByEmail;
            _createUser = createUser;
        }

        public async Task<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _getUserByEmail.Execute(command.Email);
            if (existingUser != null) throw new UserAlreadyExistsException();

            var hashedPassword = _passwordService.Hash(command.Password);
            var emailVerificationToken = _jwtService.GenerateToken(_jwtConfig.EmailVerificationSecret);
            var user = await _createUser.Execute(new UserEntity(
                command.Email,
                hashedPassword,
                emailVerificationToken));

            await SendVerificationEmail(user.Id, user.Email, user.EmailVerificationToken); // TODO: Raise event to send email instead
             
            return ToDto(user); // TODO: Automapper
        }

        private UserDto ToDto(UserEntity user)
        {
            return new UserDto(user.Id, user.Email);
        }

        private async Task SendVerificationEmail(long userId, string email, string token)
        {
            var emailSuccess = await _mailProvider.SendMailAsync(email, "Welcome", $"http://localhost:3000/user/{userId}/verify/{token}");

            if (!emailSuccess) throw new EmailVerificationException(userId, email);
        }
    }

    public class UserAlreadyExistsException : Exception { }
}
