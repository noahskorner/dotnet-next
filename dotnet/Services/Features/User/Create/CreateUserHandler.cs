using AutoMapper;
using Data.Entities.User;
using MediatR;
using Services.Configuration;
using Services.Providers.MailProvider;
using Services.Services;

namespace Services.Features.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly JwtConfiguration _jwtConfig;
        private readonly IPasswordService _passwordService;
        private readonly IMailProvider _mailProvider;
        private readonly IJwtService _jwtService;
        private readonly IGetUserByEmail _getUserByEmail;
        private readonly ICreateUser _createUser;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(
            IPasswordService passwordService,
            IMailProvider mailProvider,
            JwtConfiguration jwtConfig,
            IJwtService jwtService,
            IGetUserByEmail getUserByEmail,
            ICreateUser createUser,
            IMapper mapper)
        {
            _passwordService = passwordService;
            _mailProvider = mailProvider;
            _jwtConfig = jwtConfig;
            _jwtService = jwtService;
            _getUserByEmail = getUserByEmail;
            _createUser = createUser;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _getUserByEmail.Execute(command.Email);
            if (existingUser != null) throw new UserAlreadyExistsException();

            var hashedPassword = _passwordService.Hash(command.Password);
            var emailVerificationToken = _jwtService.GenerateToken(_jwtConfig.EmailVerificationSecret);
            var user = await _createUser.Execute(command.Email, hashedPassword, emailVerificationToken);

            await SendVerificationEmail(user.Id, user.Email, user.EmailVerificationToken); // TODO: Raise event to send email instead

            return _mapper.Map<UserDto>(user);
        }

        private async Task SendVerificationEmail(long userId, string email, string token)
        {
            var emailSuccess = await _mailProvider.SendMailAsync(email, "Welcome", $"http://localhost:3000/user/{userId}/verify/{token}");

            if (!emailSuccess) throw new SendEmailVerificationException(userId, email);
        }
    }

    public class UserAlreadyExistsException : Exception { }

    public class SendEmailVerificationException : Exception
    {
        public long UserId { get; }
        public string Email { get; }
        public SendEmailVerificationException(long userId, string email) : base($"Unable to send verification email to {email} for user {userId}")
        {
            UserId = userId;
            Email = email;
        }

    }
}
