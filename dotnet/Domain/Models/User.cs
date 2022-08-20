namespace Domain.Models
{
    public class User
    {
        public long Id { get; init; }
        public string Email { get; init; }
        public string EmailVerificationToken { get; init; }
    }
}
