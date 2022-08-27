using Api.Constants;
using Api.Extensions;
using Api.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Configuration;
using Services.Features.Auth.Login;

namespace Api.Controllers.Api.Auth.Login
{
    [Route($"{ApiConstants.ROUTE_PREFIX}/auth")]
    public class LoginController : ApiController
    {
        public const string TOKEN_COOKIE_KEY = "Token";
        private readonly IMediator _mediator;
        private readonly IValidator<LoginRequest> _validator;
        private readonly AppConfiguration _appConfig;

        public LoginController(
            IMediator mediator,
            IValidator<LoginRequest> validator,
            AppConfiguration appConfig)
        {
            _mediator = mediator;
            _validator = validator;
            _appConfig = appConfig;
        }

        /// <summary>
        /// Creates a login attempt, sets the refresh token secure cookie, and returns the access token.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]

        public async Task<IActionResult> Post([FromBody] LoginRequest request)
        {
            await _validator.ValidateAsyncOrThrow(request);

            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);

            var cookieOptions = GetTokenCookieOptions(result.RefreshTokenExpiration);
            Response.Cookies.Append(TOKEN_COOKIE_KEY, result.RefreshToken, cookieOptions);
            var response = new LoginResponse(result.RefreshToken);

            return Created(new Result<LoginResponse>(response));
        }

        private CookieOptions GetTokenCookieOptions(DateTime refreshTokenExpiration) => 
            new CookieOptions() { Domain = _appConfig.BackendDomain, Secure = true, Expires = refreshTokenExpiration };
    }
}
