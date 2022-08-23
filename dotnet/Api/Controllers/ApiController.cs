using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Api.Controllers
{
    public class ApiController : ControllerBase
    {
        public CreatedResult Created([ActionResultObjectValue] object? value)
        {
            return Created(string.Empty, value);
        }
    }
}
