using Api.Constants;
using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Users;
using Services.Features.Users.Get;

namespace Api.Controllers.Api.Users.Get
{
    [Route($"{ApiConstants.ROUTE_PREFIX}/user")]
    [ApiController]
    public class GetUserController : ApiController
    {
        private readonly IMediator _mediator;

        public GetUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get([FromRoute] long userId)
        {
            var command = new GetUserCommand(userId, User!.Id);
            var result = await _mediator.Send(command);

            return Ok(new Result<UserDto>(result));
        }
    }
}
