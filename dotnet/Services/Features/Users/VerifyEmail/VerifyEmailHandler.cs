using AutoMapper;
using Data.Repositories.Users;
using MediatR;
using Services.Configuration;
using Services.Services;

namespace Services.Features.Users.VerifyEmail
{
    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, UserDto>
    {
        private readonly IGetUserById _getUserById;
        private readonly JwtConfiguration _jwtConfig;
        private readonly IJwtService _jwtService;
        private readonly IUpdateIsEmailVerified _updateIsEmailVerified;
        private readonly IMapper _mapper;

        public VerifyEmailHandler(
            IGetUserById getUserById,
            JwtConfiguration jwtConfig,
            IJwtService jwtService,
            IUpdateIsEmailVerified updateIsEmailVerified,
            IMapper mapper)
        {
            _getUserById = getUserById;
            _jwtConfig = jwtConfig;
            _jwtService = jwtService;
            _updateIsEmailVerified = updateIsEmailVerified;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(VerifyEmailCommand command, CancellationToken cancellationToken)
        {
            var user = await _getUserById.Execute(command.UserId);
            if (user == null) throw new VerifyEmailUserNotFoundException(command.UserId);
            if (user.EmailVerificationToken != command.Token) throw new VerifyEmailInvalidTokenException(command.UserId, command.Token);

            var isValidToken = ValidateEmailVerificationToken(command.Token);
            if (!isValidToken) throw new VerifyEmailInvalidTokenException(command.UserId, command.Token);

            var updatedUser = await _updateIsEmailVerified.Execute(command.UserId, true);

            return _mapper.Map<UserDto>(updatedUser);
        }

        private bool ValidateEmailVerificationToken(string token)
        {
            var request = new ValidateTokenRequest(token, _jwtConfig.EmailVerificationSecret, false);
            return _jwtService.ValidateToken(request);
        }
    }

    public class VerifyEmailUserNotFoundException : Exception
    {
        public long UserId { get; }

        public VerifyEmailUserNotFoundException(long userId) : base($"User {userId} was not found.")
        {
            UserId = userId;
        }
    }

    public class VerifyEmailInvalidTokenException : Exception
    {
        public long UserId { get; }
        public string Token { get; }

        public VerifyEmailInvalidTokenException(long userId, string token) : base($"User {userId} provided an invalid EmailVerificationToken")
        {
            UserId = userId;
            Token = token;
        }
    }
}
