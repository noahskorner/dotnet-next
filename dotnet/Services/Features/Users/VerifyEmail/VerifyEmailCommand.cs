using MediatR;

namespace Services.Features.Users.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<UserDto>
    {
        public long UserId { get; }
        public string Token { get; }

        public VerifyEmailCommand(long userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}
