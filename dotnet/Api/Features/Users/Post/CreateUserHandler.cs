using Api.Data;
using Api.Domain.Users;
using Api.Services.MailProvider;
using Api.Services.PasswordManager;
using MediatR;

namespace Api.Features.Users.Post
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly ApiContext _context;
        private readonly IPasswordManager _passwordManager;
        private readonly IMailProvider _mailProvider;

        public CreateUserCommandHandler(
            ApiContext context,
            IPasswordManager passwordManager,
            IMailProvider mailProvider)
        {
            _context = context;
            _passwordManager = passwordManager;
            _mailProvider = mailProvider;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var hashedPassword = _passwordManager.Hash(request.Password);

            var user = new User()
            {
                Email = request.Email,
                Password = hashedPassword,
                EmailVerificationToken = "here is me"
            };

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            await SendEmailVerification(user.Id, user.Email);

            return user.ToDto();
        }

        private async Task SendEmailVerification(long userId, string email)
        {
            await _mailProvider.SendMailAsync(email, "Welcome", $"{userId}");
        }
    }
}
