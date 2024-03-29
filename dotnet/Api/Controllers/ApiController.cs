﻿using Domain.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Api.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        protected new User? User
        {
            get
            {
                return ControllerContext.HttpContext.Items["User"] as User ?? null;
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected CreatedResult Created([ActionResultObjectValue] object? value)
        {
            return Created(string.Empty, value);
        }
    }
}
