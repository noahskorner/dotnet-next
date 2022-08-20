using Api.Models;
using Domain.Features.User;
using Domain.Features.User.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Api.User.Create
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
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var command = new CreateUserCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);
            return Created("", new Result<UserDto>(result));
        }
    }
}
