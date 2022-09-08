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

        public static Task<Result<T>> AsNotFound<T>(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.AsHttpStatusCode<T>(HttpStatusCode.NotFound);
        }

        public static Task<Result<T>> AsForbidden<T>(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.AsHttpStatusCode<T>(HttpStatusCode.Forbidden);
        }

        public static Task<Result<T>> AsUnauthorized<T>(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.AsHttpStatusCode<T>(HttpStatusCode.Unauthorized);
        }

        public static Task AsUnauthorized(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.AsHttpStatusCode(HttpStatusCode.Unauthorized);
        }

        public static Task<Result<T>> AsOk<T>(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.AsHttpStatusCode<T>(HttpStatusCode.OK);
        }

        public static Task AsOk(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.AsHttpStatusCode(HttpStatusCode.OK);
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

        public static async Task AsHttpStatusCode(this Task<HttpResponseMessage> responseTask, HttpStatusCode statusCode)
        {
            var response = await responseTask;
            if (response == null) throw new Exception();

            Assert.That(response.StatusCode, Is.EqualTo(statusCode));
        }
    }
}