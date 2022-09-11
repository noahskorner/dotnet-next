using AutoMapper;
using Data.Repositories.Users;
using Domain.Models.Users;
using MediatR;
using Services.Configuration;
using Services.Features.Auth;
using System.Security.Claims;

namespace Services.Features.Users.Create
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly JwtConfiguration _jwtConfig;
        private readonly IJwtService _jwtService;
        private readonly IGetUserByEmail _getUserByEmail;
        private readonly ICreateUser _createUser;
        private readonly IMapper _mapper;
        private readonly IPublisher _publisher;

        public CreateUserHandler(
            JwtConfiguration jwtConfig,
            IJwtService jwtService,
            IGetUserByEmail getUserByEmail,
            ICreateUser createUser,
            IMapper mapper,
            IPublisher publisher)
        {
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


            var hashedPassword = User.HashPassword(command.Password);
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
