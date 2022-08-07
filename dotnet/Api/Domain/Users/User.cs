namespace Api.Domain.Users
{
    public class User : Auditable
    {
        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }
        public string EmailVerificationToken { get; set; }
        public bool IsEmailVerified { get; }

    }
}
