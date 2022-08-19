using Domain.Features.User;
using Domain.Features.User.Create;
using Domain.Models;
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
            try
            {
                var createUserRequest = ToCommand(request); // TODO: Automapper
                var result = await _mediator.Send(createUserRequest);
                return Created("", new Result<UserDto>(result));
            }
            catch (UserAlreadyExistsException)
            {
                return BadRequest(new Result<UserDto>());
            }
        }

        private static CreateUserCommand ToCommand(CreateUserRequest request)
        {
            return new CreateUserCommand(request.Email, request.Password);
        }
    }
}
