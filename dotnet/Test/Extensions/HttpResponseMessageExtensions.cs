using Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace Test.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static Task<Result<T>> AsBadRequest<T>(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.AsHttpStatusCode<T>(HttpStatusCode.BadRequest);
        }

        public static Task<Result<T>> AsCreated<T>(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.AsHttpStatusCode<T>(HttpStatusCode.Created);
        }

        public static async Task<Result<T>> AsHttpStatusCode<T>(this Task<HttpResponseMessage> responseTask, HttpStatusCode statusCode)
        {
            var response = await responseTask;
            if (response == null) throw new Exception();

            Assert.That(response.StatusCode, Is.EqualTo(statusCode));

            var result = await response.Content.ReadFromJsonAsync<Result<T>>();
            if (result == null)
            {
                throw new ArgumentNullException($"{nameof(HttpResponseMessage.Content)} was null");
            }

            Assert.IsInstanceOf(typeof(Result<T>), result);

            return result;
        }
    }
}