﻿using Domain.Models.Users;
using Infrastructure.Services;
using Services.Configuration;
using System.Security.Claims;

namespace Services.Features.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IDateService _dateService;
        private readonly IJwtService _jwtService;
        private readonly JwtConfiguration _jwtConfig;

        public AuthService(
            IDateService dateService,
            JwtConfiguration jwtConfig,
            IJwtService jwtService)
        {
            _dateService = dateService;
            _jwtConfig = jwtConfig;
            _jwtService = jwtService;
        }

        public string GenerateAccessToken(long userId, string email, IEnumerable<Role> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
            };
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x.Name)));

            var expiresIn = _dateService.Now() + _jwtConfig.AccessTokenExpiresIn;
            var request = new GenerateTokenRequest(_jwtConfig.AccessTokenSecret, claims, expiresIn);

            return _jwtService.GenerateToken(request);
        }

        public (string, DateTimeOffset) GenerateRefreshToken(long userId, string email)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
            };
            var expiresIn = _dateService.Now() + _jwtConfig.RefreshTokenExpiresIn;
            var request = new GenerateTokenRequest(_jwtConfig.RefreshTokenSecret, claims, expiresIn);

            return (_jwtService.GenerateToken(request), expiresIn);
        }
    }
}
