using MediatR;

namespace Api.Features.Users.Create
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
