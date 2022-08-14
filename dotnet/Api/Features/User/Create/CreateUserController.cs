using Api.Domain.User;
using Api.Extensions;
using Api.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.User.Create
{
    [Route("api/user")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        private readonly IValidator<CreateUserRequest> _validator;
        private readonly IMediator _mediator;

        public CreateUserController(
            IValidator<CreateUserRequest> validator,
            IMediator mediator)
        {
            _validator = validator;
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request).ToResult<UserDto>();
            if (validationResult.Errors.Any()) return BadRequest(validationResult);

            var result = await _mediator.Send(request.ToCommand());
            return Created("", new Result<UserDto>(result));
        }
    }
}
