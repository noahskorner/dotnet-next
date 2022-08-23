namespace Domain.Models
{
    public class User
    {
        public long Id { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string EmailVerificationToken { get; private set; }
        public bool IsEmailVerified { get; private set; }
    }
}
