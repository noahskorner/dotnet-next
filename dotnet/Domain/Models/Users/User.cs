namespace Domain.Models.Users
{
    public partial class User
    {
        public long Id { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string EmailVerificationToken { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public IEnumerable<Role> Roles { get; private set; }
    }
}
