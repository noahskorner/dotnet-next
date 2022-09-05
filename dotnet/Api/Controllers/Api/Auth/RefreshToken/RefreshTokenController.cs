using Api.Constants;
using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Configuration;
using Services.Features.Auth.RefreshToken;

namespace Api.Controllers.Api.Auth.RefreshToken
{
    [Route($"{ApiConstants.ROUTE_PREFIX}/auth")]
    public class RefreshTokenController : AuthController
    {
        private readonly IMediator _mediator;

        public RefreshTokenController(
            AppConfiguration appConfig,
            IMediator mediator) : base(appConfig)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Validates the refresh token, resets the refresh token secure cookie, and returns a new access token.
        /// </summary>
        [HttpPut]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put()
        {
            var command = new RefreshTokenCommand(RefreshToken);
            var result = await _mediator.Send(command);

            SetRefreshTokenCookie(Response, result.RefreshToken, result.RefreshTokenExpiration);
            var response = new AuthResponse(result.RefreshToken);
            return Ok(new Result<AuthResponse>(response));
        }
    }
}
