using Api.Features.Users;

namespace Api.Domain.Users
{
    public static class UserExtensions
    {
        public static UserDto ToDto(this User user) => new UserDto(user.Id, user.Email);
    }
}
