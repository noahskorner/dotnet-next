using MediatR;

namespace Services.Features.Users.Get
{
    public class GetUserCommand : IRequest<UserDto>
    {
        public long UserId { get; }
        public long CurrentUserId { get; }

        public GetUserCommand(long userId, long currentUserId)
        {
            UserId = userId;
            CurrentUserId = currentUserId;
        }
    }
}
