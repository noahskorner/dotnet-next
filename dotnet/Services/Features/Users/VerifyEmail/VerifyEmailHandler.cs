﻿using AutoMapper;
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
            if (user == null) throw new UserNotFoundException(command.UserId);
            if (user.EmailVerificationToken != command.Token) throw new InvalidEmailVerificationTokenException(command.UserId, command.Token);

            var isValidToken = _jwtService.ValidateToken(command.Token, _jwtConfig.EmailVerificationSecret);
            if (!isValidToken) throw new InvalidEmailVerificationTokenException(command.UserId, command.Token);

            var updatedUser = await _updateIsEmailVerified.Execute(command.UserId, true);

            return _mapper.Map<UserDto>(updatedUser);
        }
    }

    public class UserNotFoundException : Exception
    {
        public long UserId { get; }

        public UserNotFoundException(long userId) : base($"User {userId} was not found.")
        {
            UserId = userId;
        }
    }

    public class InvalidEmailVerificationTokenException : Exception
    {
        public long UserId { get; }
        public string Token { get; }

        public InvalidEmailVerificationTokenException(long userId, string token) : base($"User {userId} provided an invalid EmailVerificationToken")
        {
            UserId = userId;
            Token = token;
        }
    }
}