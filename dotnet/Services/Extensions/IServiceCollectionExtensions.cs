using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Data.Extensions;
using Services.Providers.MailProvider;
using Services.Configuration;
using Services.Services;

namespace Services.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.RegisterDatabase(configuration);
            services.AddConfiguration(configuration);
            services.AddProviders();
            services.AddServices();
            services.AddPipelineBehaviors();

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton((sp) => configuration.GetSection(SmtpConfiguration.Smtp).Get<SmtpConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(JwtConfiguration.Jwt).Get<JwtConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(AppConfiguration.App).Get<AppConfiguration>());

            return services;
        }

        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddSingleton<IMailProvider, SystemMailProvider>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IDateService, DateService>();

            return services;
        }

        public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services)
        {
            return services;
        }
    }
}
