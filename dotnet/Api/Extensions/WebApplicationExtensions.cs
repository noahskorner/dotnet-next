using Api.Models;
using Data;
using Services.Constants;
using Services.Enumerations;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using System.Text.Json;
using Services.Features.Users.Create;

namespace Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void BuildApi(this WebApplication app)
        {
            app.RunMigrations();
            app.UseSwaggerPage();
            app.UseHttpsRedirection();
            app.UseDefaultExceptionHandler();
            app.MapControllers();
        }

        public static void UseSwaggerPage(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        public static void UseDefaultExceptionHandler(this WebApplication app)
        {
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            app.UseExceptionHandler(x =>
            {
                x.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    var errors = new List<Error>();
                    var statusCode = (int)HttpStatusCode.InternalServerError;

                    switch (exception)
                    {
                        case ValidationException validationException:
                            statusCode = (int)HttpStatusCode.BadRequest;
                            errors = validationException.Errors
                               .Select(error => new Error(
                                   ErrorType.Validation,
                                   error.ErrorMessage,
                                   field: error.PropertyName))
                               .ToList();
                            break;
                        case UserAlreadyExistsException userAlreadyExistsException:
                            statusCode = (int)HttpStatusCode.BadRequest;
                            errors.Add(new Error(ErrorType.Exception, Errors.USER_ALREADY_EXISTS, key: nameof(Errors.USER_ALREADY_EXISTS)));
                            break;
                        default:
                            errors.Add(new Error(ErrorType.Exception, Errors.UNKNOWN, key: nameof(Errors.UNKNOWN)));
                            break;
                    }

                    var result = new Result<object>(errors: errors);
                    var resultJson = JsonSerializer
                        .Serialize(result, jsonSerializerOptions);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = statusCode;


                    await context.Response.WriteAsync(resultJson, Encoding.UTF8);
                });
            });
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        public static void RunMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
                if (!context.Database.IsInMemory())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
