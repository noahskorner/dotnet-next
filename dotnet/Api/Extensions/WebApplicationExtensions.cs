using Api.Constants;
using Api.Enumerations;
using Api.Extensions;
using Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Api.Extensions
{
    public static class WebApplicationExtensions
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public static void UseFluentValidationExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(x =>
            {
                x.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    #pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var exception = errorFeature.Error;
                    #pragma warning restore CS8602 // Dereference of a possibly null reference.

                    if (exception is ValidationException validationException)
                    {
                        var errors = validationException.Errors
                            .Select(error => new Error(
                                ErrorType.Validation,
                                error.ErrorMessage,
                                field: error.PropertyName))
                            .ToList();
                        var result = new Result<object>(errors: errors);
                        var resultJson = JsonSerializer
                            .Serialize(result, _jsonSerializerOptions);
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(resultJson, Encoding.UTF8);
                    }
                });
            });
        }

        public static void UseExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(x =>
            {
                x.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    #pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var exception = errorFeature.Error;
                    #pragma warning restore CS8602 // Dereference of a possibly null reference.

                    var errors = new List<Error> { new Error(ErrorType.Exception, Errors.UNKNOWN, key: nameof(Errors.UNKNOWN)) };
                    var result = new Result<object>(errors: errors);
                    var resultJson = JsonSerializer
                        .Serialize(result, _jsonSerializerOptions);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(resultJson, Encoding.UTF8);
                });
            });
        }
    }
}
