using Api.Data;
using Api.Features.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var result = await _mediator.Send(request.ToCommand());
            return StatusCode(201, result);
        }
    }

    public class CreateUserRequest
    {
        public string Email { get; }
        public string Password { get; }

        public CreateUserRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public static class CreateUserRequestExtensions
    {
        public static CreateUserCommand ToCommand(this CreateUserRequest request) => new CreateUserCommand(request.Email, request.Password);
    }

}
