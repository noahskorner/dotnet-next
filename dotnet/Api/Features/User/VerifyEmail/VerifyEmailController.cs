using Microsoft.AspNetCore.Mvc;

namespace Api.Features.User.VerifyEmail
{
    [Route("api/user/:userId/verify-email")]
    [ApiController]
    public class VerifyEmailController : ControllerBase
    {
    }
}
