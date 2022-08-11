namespace Api.Domain.User
{
    public class UserEntity : Auditable
    {
        public UserEntity(string email, string password)
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
