namespace Api.Domain.Users
{
    public class User : Auditable
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmailVerificationToken { get; set; }
        public string IsEmailVerified{ get; set; }
    }
}
