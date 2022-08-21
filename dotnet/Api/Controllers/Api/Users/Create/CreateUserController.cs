using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Services.Features.Users;
using Services.Features.Users.Create;

namespace Api.Controllers.Api.Users.Create
{
    [Route("api/user")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer _localizer;

        public CreateUserController(
            IMediator mediator,
            IStringLocalizer localizer)
        {
            _mediator = mediator;
            _localizer = localizer;
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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_localizer.GetString("Test"));
        }
    }
}
