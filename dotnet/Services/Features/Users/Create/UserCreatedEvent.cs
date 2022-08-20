using MediatR;

namespace Services.Features.Users.Create
{
    public class UserCreatedEvent : INotification
    {
        public long UserId { get; }
        public string Email { get; }
        public string EmailVerificationToken { get; }

        public UserCreatedEvent(long userId, string email, string emailVerificationToken)
        {
            UserId = userId;
            Email = email;
            EmailVerificationToken = emailVerificationToken;
        }
    }
}
