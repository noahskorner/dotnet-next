namespace Api.Features.User.Create
{
    public class CreateUserRequest
    {
        public string Email { get; }
        public string Password { get; }

        public CreateUserRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public static class CreateUserRequestExtensions
    {
        public static CreateUserCommand ToCommand(this CreateUserRequest request) => new CreateUserCommand(request.Email, request.Password);
    }
}
