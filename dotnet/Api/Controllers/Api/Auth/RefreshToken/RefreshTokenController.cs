using Api.Constants;
using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Auth.RefreshToken;

namespace Api.Controllers.Api.Auth.RefreshToken
{
    [Route($"{ApiConstants.ROUTE_PREFIX}/auth")]
    public class RefreshTokenController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IAuthHelper _authService;

        public RefreshTokenController(
            IMediator mediator,
            IAuthHelper authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

        /// <summary>
        /// Validates the refresh token, resets the refresh token secure cookie, and returns a new access token.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]

        public async Task<IActionResult> Put()
        {
            var refreshToken = _authService.GetRefreshToken(Request);
            var command = new RefreshTokenCommand(refreshToken);
            var result = await _mediator.Send(command);

            _authService.SetRefreshTokenCookie(Response, result.RefreshToken, result.RefreshTokenExpiration);
            var response = new AuthResponse(result.RefreshToken);
            return Ok(new Result<AuthResponse>(response));
        }
    }
}
