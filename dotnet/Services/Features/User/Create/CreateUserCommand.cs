using MediatR;

namespace Services.Features.User.Create
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
}
