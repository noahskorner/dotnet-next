namespace Api.Features.Users.Create
{
    public class EmailVerificationException : Exception
    {
        public EmailVerificationException(long userId, string email) : base($"Unable to send verification email to {email} for user {userId}")
        {
            UserId = userId;
            Email = email;
        }

        public long UserId { get; }
        public string Email { get; }
    }
}
