using Api.Domain.User;
using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.User.Create
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
            var result = await _mediator.Send(request.ToCommand());
            return StatusCode(201, new Result<UserDto>(result));
        }
    }
}
