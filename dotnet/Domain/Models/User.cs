namespace Domain.Models
{
    public class User
    {
        public long Id { get; private set; }
        public string Email { get; private set; }
        public string EmailVerificationToken { get; private set; }
    }
}
