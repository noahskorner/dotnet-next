using Data.Entities;

namespace Data.Entities.User
{
    public class UserEntity : Auditable
    {
        public UserEntity(
            string email,
            string password,
            string emailVerificationToken)
        {
            Email = email;
            Password = password;
            EmailVerificationToken = emailVerificationToken;
        }

        public string Email { get; }
        public string Password { get; }
        public string EmailVerificationToken { get; set; }
        public bool IsEmailVerified { get; }
    }
}
