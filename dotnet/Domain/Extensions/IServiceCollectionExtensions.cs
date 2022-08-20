using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Domain.Configuration;
using Domain.Providers.MailProvider;
using Domain.PipelineBehaviors;
using Data.Extensions;
using Domain.Services;
using Domain.Features.User;

namespace Domain.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDomain(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.RegisterDatabase(configuration);
            services.AddConfiguration(configuration);
            services.AddMiddleware();
            services.AddProviders();
            services.AddServices();
            services.AddPipelineBehaviors();

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton((sp) => configuration.GetSection(SmtpConfiguration.Smtp).Get<SmtpConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(JwtConfiguration.Jwt).Get<JwtConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(FrontendConfiguration.Frontend).Get<FrontendConfiguration>());

            return services;
        }

        public static IServiceCollection AddMiddleware(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddSingleton<IMailProvider, SystemMailProvider>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IPasswordService, PasswordService>();

            return services;
        }

        public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
