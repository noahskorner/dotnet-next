using Api.Constants;
using Api.Extensions;
using Api.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Configuration;
using Services.Features.Auth.Login;

namespace Api.Controllers.Api.Auth.Login
{
    [Route($"{ApiConstants.ROUTE_PREFIX}/auth")]
    public class LoginController : AuthController
    {
        private readonly IMediator _mediator;
        private readonly IValidator<LoginRequest> _validator;

        public LoginController(
            AppConfiguration appConfig,
            IMediator mediator,
            IValidator<LoginRequest> validator) : base(appConfig)
        {
            _mediator = mediator;
            _validator = validator;
        }

        /// <summary>
        /// Creates a login attempt, sets the refresh token secure cookie, and returns the access token.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]

        public async Task<IActionResult> Post([FromBody] LoginRequest request)
        {
            await _validator.ValidateAsyncOrThrow(request);
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);

            SetRefreshTokenCookie(Response, result.RefreshToken, result.RefreshTokenExpiration);
            var response = new AuthResponse(result.AccessToken);
            return Created(new Result<AuthResponse>(response));
        }
    }
}
