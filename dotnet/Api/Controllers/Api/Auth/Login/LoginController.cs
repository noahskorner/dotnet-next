using Api.Constants;
using Api.Extensions;
using Api.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Auth.Login;

namespace Api.Controllers.Api.Auth.Login
{
    [Route($"{ApiConstants.ROUTE_PREFIX}/auth")]
    public class LoginController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IValidator<LoginRequest> _validator;
        private readonly IAuthHelper _authService;

        public LoginController(
            IMediator mediator,
            IValidator<LoginRequest> validator,
            IAuthHelper authService)
        {
            _mediator = mediator;
            _validator = validator;
            _authService = authService;
        }

        /// <summary>
        /// Creates a login attempt, sets the refresh token secure cookie, and returns the access token.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]

        public async Task<IActionResult> Post([FromBody] LoginRequest request)
        {
            await _validator.ValidateAsyncOrThrow(request);
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);

            _authService.SetRefreshTokenCookie(Response, result.RefreshToken, result.RefreshTokenExpiration);
            var response = new AuthResponse(result.RefreshToken);
            return Created(new Result<AuthResponse>(response));
        }
    }
}
