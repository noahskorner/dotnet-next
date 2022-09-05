﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Api.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public CreatedResult Created([ActionResultObjectValue] object? value)
        {
            return Created(string.Empty, value);
        }
    }
}
