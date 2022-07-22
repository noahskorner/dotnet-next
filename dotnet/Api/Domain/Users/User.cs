using Api.Data;

namespace Api.Domain.Users
{
    public class User : Auditable
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
