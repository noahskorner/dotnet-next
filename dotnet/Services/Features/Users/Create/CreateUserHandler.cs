using AutoMapper;
using Data.Repositories.Users;
using MediatR;
using Services.Configuration;
using Services.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            if (existingUser != null) throw new CreateUserAlreadyExistsException();

            var hashedPassword = _passwordService.Hash(command.Password);
            var emailVerificationToken = GetEmailVerificationToken(command.Email);
            var user = await _createUser.Execute(command.Email, hashedPassword, emailVerificationToken);

            await _publisher.Publish(new UserCreatedEvent(user.Id, user.Email, user.EmailVerificationToken));

            return _mapper.Map<UserDto>(user);
        }

        private string GetEmailVerificationToken(string email)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email),
            };
            var request = new GenerateTokenRequest(_jwtConfig.EmailVerificationSecret, claims, null);

            return _jwtService.GenerateToken(request);
        }
    }

    public class CreateUserAlreadyExistsException : Exception { }
}
