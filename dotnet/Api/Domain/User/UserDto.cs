namespace Api.Domain.User
{
    public class UserDto
    {
        public long Id { get; }
        public string Email { get; }

        public UserDto(long id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}
