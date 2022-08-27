using Api.Models;
using Data;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using System.Text.Json;
using Services.Features.Users.Create;
using Services.Features.Users.VerifyEmail;
using Api.Enumerations;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Api.Localization;
using Services.Features.Auth.Login;
using Microsoft.Extensions.Localization;
using Api.Constants;

namespace Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void BuildApi(this WebApplication app)
        {
            app.RunMigrations();
            app.UseSwaggerPage();
            app.UseLocalization();
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

        public static void UseDefaultExceptionHandler(this WebApplication app)
        {
            var localizer = app.Services.GetRequiredService<IStringLocalizer>();
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
                    var exception = errorFeature?.Error;

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
                        case CreateUserAlreadyExistsException createUserAlreadyExistsException:
                            statusCode = (int)HttpStatusCode.BadRequest;
                            errors.Add(new Error(ErrorType.Exception, localizer.GetString(Errors.CREATE_USER_AREADY_EXISTS), key: Errors.CREATE_USER_AREADY_EXISTS));
                            break;
                        case VerifyEmailUserNotFoundException verifyEmailUserNotFoundException:
                            statusCode = (int)HttpStatusCode.NotFound;
                            errors.Add(new Error(ErrorType.Exception, localizer.GetString(Errors.CREATE_USER_NOT_FOUND), key: Errors.CREATE_USER_NOT_FOUND));
                            break;
                        case VerifyEmailInvalidTokenException verifyEmailInvalidTokenException:
                            statusCode = (int)HttpStatusCode.Unauthorized;
                            errors.Add(new Error(ErrorType.Exception, localizer.GetString(Errors.VERIFY_EMAIL_INVALID_TOKEN), key: Errors.VERIFY_EMAIL_INVALID_TOKEN));
                            break;
                        case LoginUserNotFoundException userNotFoundException:
                        case LoginInvalidPasswordException invalidPasswordException:
                            statusCode = (int)HttpStatusCode.Unauthorized;
                            errors.Add(new Error(ErrorType.Exception, localizer.GetString(Errors.LOGIN_USER_INVALID_EMAIL_OR_PASSWORD), key: Errors.LOGIN_USER_INVALID_EMAIL_OR_PASSWORD));
                            break;
                        default:
                            errors.Add(new Error(ErrorType.Exception, localizer.GetString(Errors.UNKNOWN), key: Errors.UNKNOWN));
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

        public static void UseLocalization(this WebApplication app)
        {
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
            };
            app.UseRequestLocalization(options);
            app.UseMiddleware<LocalizationMiddleware>();
        }

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
