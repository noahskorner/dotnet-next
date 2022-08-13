using Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace Api.Test.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<Result<T>> AsBadRequest<T>(this HttpClient httpClient, string requestUri, object? value)
        {
            var response = await httpClient.PostAsJsonAsync(requestUri, value);
            if (response == null) throw new Exception();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var result = await response.Content.ReadFromJsonAsync<Result<T>>();
            if (result == null)
            {
                throw new ArgumentNullException($"{nameof(HttpResponseMessage.Content)} was null");
            }

            return result;
        }
    }
}
