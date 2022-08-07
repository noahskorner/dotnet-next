using Api.Configuration;
using Api.Data;
using Api.Domain.Users;
using Api.Services.JwtService;
using Api.Services.MailService;
using Api.Services.PasswordService;
using MediatR;

namespace Api.Features.Users.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly ApiContext _context;
        private readonly JwtConfiguration _jwtConfig;
        private readonly IPasswordService _passwordService;
        private readonly IMailService _mailService;
        private readonly IJwtService _jwtService;

        public CreateUserCommandHandler(
            ApiContext context,
            IPasswordService passwordService,
            IMailService mailService,
            JwtConfiguration jwtConfig,
            IJwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _mailService = mailService;
            _jwtConfig = jwtConfig;
            _jwtService = jwtService;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var hashedPassword = _passwordService.Hash(request.Password);

            var user = new User(request.Email, hashedPassword);

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            var emailVerificationToken = await SendVerificationEmail(user.Id, user.Email);
            user.EmailVerificationToken = emailVerificationToken;
            await _context.SaveChangesAsync();

            return user.ToDto();
        }

        private async Task<string> SendVerificationEmail(long userId, string email)
        {
            var token = _jwtService.GenerateToken(_jwtConfig.EmailVerificationSecret);
            var emailSuccess = await _mailService.SendMailAsync(email, "Welcome", $"http://localhost:3000/user/{userId}/verify/{token}");

            if (!emailSuccess) throw new EmailVerificationException(userId, email);

            return token;
        }
    }
}
