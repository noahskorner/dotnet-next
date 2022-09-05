using Data.Repositories.Users;
using Domain.Models;
using Services.Configuration;
using Services.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers.Api.Auth
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, JwtConfiguration jwtConfig, IJwtService jwtService, IGetUserById getUserById)
        {
            var token = context.Request.Headers["Authorization"].First().Split(" ").Last();
            var request = new ValidateTokenRequest(token, jwtConfig.AccessTokenSecret, true);

            var jwtSecurityToken = jwtService.ValidateToken(request);
            var user = await GetUserFromToken(jwtSecurityToken, getUserById);
            if (user != null)
            {
                context.Items["User"] = user;
            }

            await _next(context);
        }

        private async Task<User?> GetUserFromToken(JwtSecurityToken? jwtSecurityToken, IGetUserById getUserById)
        {
            if (jwtSecurityToken == null) return null;

            var userIdClaim = jwtSecurityToken.Claims.Single(x => x.ValueType == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return null;

            if (!long.TryParse(userIdClaim.Value, out long userId)) return null;

            var user = await getUserById.Execute(userId);
            return user;
        }
    }
}
