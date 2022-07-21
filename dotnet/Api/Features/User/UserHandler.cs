using Api.Data;
using Api.Domain;
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

        public CreateUserCommandHandler(ApiContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Api.Domain.User() { 
                Email = request.Email,
                Password = request.Password
            };

            var createUserResult = await _context.User.AddAsync(user);

            return createUserResult.Entity.ToDto();
        }
    }
}
