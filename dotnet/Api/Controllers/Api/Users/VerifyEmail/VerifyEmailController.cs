using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Users;
using Services.Features.Users.VerifyEmail;

namespace Api.Controllers.Api.Users.VerifyEmail
{
    [Route("api/user/{userId}/verify-email")]
    [ApiController]
    public class VerifyEmailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VerifyEmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Verifies a users email
        /// </summary>
        [HttpPut("{token}")]

        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromRoute] long userId, [FromRoute] string token)
        {
            var command = new VerifyEmailCommand(userId, token);
            var result = await _mediator.Send(command);
            return Ok(new Result<UserDto>(result));
        }
    }
}
