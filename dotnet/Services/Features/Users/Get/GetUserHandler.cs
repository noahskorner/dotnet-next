using AutoMapper;
using Data.Repositories.Users;
using MediatR;

namespace Services.Features.Users.Get
{
    public class GetUserHandler : IRequestHandler<GetUserCommand, UserDto>
    {
        private readonly IGetUserById _getUserById;
        private readonly IMapper _mapper;

        public GetUserHandler(
            IGetUserById getUserById,
            IMapper mapper)
        {
            _getUserById = getUserById;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserCommand command, CancellationToken cancellationToken)
        {
            if (command.CurrentUserId != command.UserId)
            {
                throw new UserIdDoesNotMatchRouteIdException(command.UserId, command.UserId);
            }

            var user = await _getUserById.Execute(command.UserId);
            if (user == null) throw new ArgumentNullException();

            return _mapper.Map<UserDto>(user);
        }
    }

    public class UserIdDoesNotMatchRouteIdException : Exception
    {
        public long UserId { get; }
        public long RouteId { get; }

        public UserIdDoesNotMatchRouteIdException(long userId, long routeId) : base($"User {userId} does not have permission to get user {routeId}")
        {
            UserId = userId;
            RouteId = routeId;
        }
    }
}
