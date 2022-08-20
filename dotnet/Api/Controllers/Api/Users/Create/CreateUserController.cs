using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Users;
using Services.Features.Users.Create;

namespace Api.Controllers.Api.Users.Create
{
    [Route("api/user")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CreateUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(CreateUserRequest request)
        {
            var command = new CreateUserCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);
            return Created("", new Result<UserDto>(result));
        }
    }
}
