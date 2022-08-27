using Api.Constants;
using Api.Extensions;
using Api.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Users;
using Services.Features.Users.Create;

namespace Api.Controllers.Api.Users.Create
{
    [Route($"{ApiConstants.ROUTE_PREFIX}/user")]
    [ApiController]
    public class CreateUserController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateUserRequest> _validator;

        public CreateUserController(
            IMediator mediator,
            IValidator<CreateUserRequest> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(CreateUserRequest request)
        {
            await _validator.ValidateAsyncOrThrow(request);

            var command = new CreateUserCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);

            return Created($"https://localhost:5000/v1/{result.Id}", new Result<UserDto>(result)); // TODO: Find a more elegant way to do this
        }
    }
}
