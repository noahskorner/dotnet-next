using Api.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Api.Auth.Logout
{
    [Route($"{ApiConstants.ROUTE_PREFIX}/auth")]
    public class LogoutController : ApiController
    {
        /// <summary>
        /// Removes the secure refresh token cookie from the headers.
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public IActionResult Delete()
        {
            Response.Cookies.Delete(ApiConstants.TOKEN_COOKIE_KEY);

            return Ok();
        }
    }
}
