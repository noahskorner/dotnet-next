using Api.Constants;
using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Users;
using Services.Features.Users.VerifyEmail;

namespace Api.Controllers.Api.Users.VerifyEmail
{
    [Route($"{ApiConstants.ROUTE_PREFIX}/user/{{userId}}/verify-email")]
    [ApiController]
    public class VerifyEmailController : ApiController
    {
        private readonly IMediator _mediator;

        public VerifyEmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Verifies a users email.
        /// </summary>
        [HttpPut("{token}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromRoute] long userId, [FromRoute] string token)
        {
            var command = new VerifyEmailCommand(userId, token);
            var result = await _mediator.Send(command);
            return Ok(new Result<UserDto>(result));
        }
    }
}
