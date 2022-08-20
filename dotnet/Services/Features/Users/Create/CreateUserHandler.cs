using AutoMapper;
using Data.Entities.Users;
using MediatR;
using Services.Configuration;
using Services.Services;

namespace Services.Features.Users.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly JwtConfiguration _jwtConfig;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IGetUserByEmail _getUserByEmail;
        private readonly ICreateUser _createUser;
        private readonly IMapper _mapper;
        private readonly IPublisher _publisher;

        public CreateUserCommandHandler(
            IPasswordService passwordService,
            JwtConfiguration jwtConfig,
            IJwtService jwtService,
            IGetUserByEmail getUserByEmail,
            ICreateUser createUser,
            IMapper mapper,
            IPublisher publisher)
        {
            _passwordService = passwordService;
            _jwtConfig = jwtConfig;
            _jwtService = jwtService;
            _getUserByEmail = getUserByEmail;
            _createUser = createUser;
            _mapper = mapper;
            _publisher = publisher;
        }

        public async Task<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _getUserByEmail.Execute(command.Email);
            if (existingUser != null) throw new UserAlreadyExistsException();

            var hashedPassword = _passwordService.Hash(command.Password);
            var emailVerificationToken = _jwtService.GenerateToken(_jwtConfig.EmailVerificationSecret);
            var user = await _createUser.Execute(command.Email, hashedPassword, emailVerificationToken);

            await _publisher.Publish(new UserCreatedEvent(user.Id, user.Email, user.EmailVerificationToken));

            return _mapper.Map<UserDto>(user);
        }
    }

    public class UserAlreadyExistsException : Exception { }
}
