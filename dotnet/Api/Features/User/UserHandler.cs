using Api.Data;
using Api.Domain;
using Api.Utilities.PasswordManager;
using MediatR;

namespace Api.Features.User
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Email { get; }
        public string Password { get; }

        public CreateUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }   
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly ApiContext _context;
        private readonly IPasswordManager _passwordManager;

        public CreateUserCommandHandler(ApiContext context, IPasswordManager passwordManager)
        {
            _context = context;
            _passwordManager = passwordManager;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var hashedPassword = _passwordManager.Hash(request.Password);

            var user = new Api.Domain.User() { 
                Email = request.Email,
                Password = hashedPassword
            };

            var createUserResult = await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return createUserResult.Entity.ToDto();
        }
    }
}
