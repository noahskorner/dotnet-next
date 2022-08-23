using MediatR;

namespace Services.Features.Auth.Login
{
    public class LoginCommand : IRequest<AuthDto>
    {
        public string Email { get; }
        public string Password { get; }

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
