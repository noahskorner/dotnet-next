using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Api
{
    public class ValidationError
    {
        public string Field { get; }
        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }

    public class Error
    {
        public string Key { get; }
        public string Message { get; }

        public Error(string key, string message)
        {
            Key = key;
            Message = message;
        }
    }

    public static class Errors
    {
        public const string UNKNOWN = "An unkown issue has occurred. Please try again.";
    }


    public static class WebApplicationExtensions
    {
        public static void UseFluentValidationExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(x =>
            {
                x.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    if (!(exception is ValidationException validationException))
                    {
                        throw exception;
                    }

                    var errors = validationException.Errors.Select(error => new ValidationError(error.PropertyName, error.ErrorMessage));
                    var errorsJson = JsonSerializer.Serialize(errors, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true,
                        WriteIndented = true,
                    });
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(errorsJson, Encoding.UTF8);
                });
            });
        }

        public static void UseUnknownExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(x =>
            {
                x.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    var errors = new List<Error>
                    {
                        new Error(nameof(Errors.UNKNOWN), Errors.UNKNOWN),
                    };
                    var errorsJson = JsonSerializer.Serialize(errors, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true,
                        WriteIndented = true,
                    });
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(errorsJson, Encoding.UTF8);
                });
            });
        }
    }
}
