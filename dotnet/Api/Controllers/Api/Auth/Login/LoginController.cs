using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Auth;
using Services.Features.Auth.Login;

namespace Api.Controllers.Api.Auth.Login
{
    [Route("v1/auth")]
    public class LoginController : ApiController
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a login attempt, sets the refresh token secure cookie, and returns the access token.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]

        public async Task<IActionResult> Post([FromBody] LoginRequest request)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);

            Response.Cookies.Append("token", result.RefreshToken, new CookieOptions() { Domain = "localhost", Secure = true }); // TODO: Add expiration, constant for cookie key, etc.
            var response = new LoginResponse(result.RefreshToken);

            return Created(new Result<LoginResponse>(response));
        }
    }
}
