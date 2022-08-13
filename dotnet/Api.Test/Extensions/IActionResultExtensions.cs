using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Test.Extensions
{
#pragma warning disable CS8603 // Possible null reference return.
    public static class IActionResultExtensions
    {
        public static Task<Result<T>> AsBadRequest<T>(this Task<IActionResult> action) where T : class
        {
            return action.AsHttpResult<BadRequestObjectResult, T>();
        }

        public static async Task<Result<T>> AsHttpResult<THttpResult, T>(this Task<IActionResult> action) where THttpResult : ObjectResult where T : class
        {
            var response = await action;

            var statusCodeAssertionMessage = $"Expected type to be {typeof(THttpResult).Name} but was {response.GetType()}";
            Assert.IsInstanceOf<THttpResult>(action, statusCodeAssertionMessage);

            var result = response as THttpResult;
            var resultAssertionMessage = $"Expected type to be {typeof(Result<T>)} but was {JsonSerializer.Serialize(result.Value)}";
            Assert.IsInstanceOf<Result<T>>(result.Value, resultAssertionMessage);

            return result.Value as Result<T>;
        }
    }
#pragma warning restore CS8603 // Possible null reference return.
}
