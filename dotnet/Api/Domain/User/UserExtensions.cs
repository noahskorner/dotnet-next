namespace Api.Domain.User
{
    public static class UserExtensions
    {
        public static UserDto ToDto(this UserEntity user) => new UserDto(user.Id, user.Email);
    }
}
