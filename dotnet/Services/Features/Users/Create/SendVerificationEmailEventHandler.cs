using MediatR;
using Services.Providers.MailProvider;

namespace Services.Features.Users.Create
{
    public class SendVerificationEmailEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly IMailProvider _mailProvider;

        public SendVerificationEmailEventHandler(IMailProvider mailProvider)
        {
            _mailProvider = mailProvider;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            var emailSuccess = await _mailProvider
                .SendMailAsync(notification.Email, "Welcome", $"http://localhost:3000/user/{notification.UserId}/verify/{notification.EmailVerificationToken}");

            if (!emailSuccess) throw new SendVerificationEmailNotSentException(notification.UserId, notification.Email);
        }
    }

    public class SendVerificationEmailNotSentException : Exception
    {
        public long UserId { get; }
        public string Email { get; }
        public SendVerificationEmailNotSentException(long userId, string email) : base($"Unable to send verification email to {email} for user {userId}")
        {
            UserId = userId;
            Email = email;
        }

    }
}
