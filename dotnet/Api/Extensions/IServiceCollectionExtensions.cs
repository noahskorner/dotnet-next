using Api.Configuration;
using Api.Data;
using Api.Extensions;
using Api.Providers.MailProvider;
using Api.Services.JwtService;
using Api.Services.PasswordService;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json;

namespace Api.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton((sp) => configuration.GetSection(CorsConfiguration.Cors).Get<CorsConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(SqlServerConfiguration.SqlServer).Get<SqlServerConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(SmtpConfiguration.Smtp).Get<SmtpConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(JwtConfiguration.Jwt).Get<JwtConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(FrontendConfiguration.Frontend).Get<FrontendConfiguration>());

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services, ConfigurationManager configuration)
        {
            var corsConfig = configuration.GetSection(CorsConfiguration.Cors).Get<CorsConfiguration>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: corsConfig.PolicyName, policy =>
                {
                    policy.WithOrigins(corsConfig.AllowedOrigins)
                          .WithMethods(corsConfig.AllowedMethods);
                });
            });

            return services;
        }

        public static IServiceCollection AddSqlServer(this IServiceCollection services, ConfigurationManager configuration)
        {
            var sqlConfig = configuration.GetSection(SqlServerConfiguration.SqlServer).Get<SqlServerConfiguration>();

            services
                .AddDbContextPool<ApiContext>(options => options
                    .UseSqlServer(sqlConfig.ConnectionString, x => x.EnableRetryOnFailure())
                    .EnableSensitiveDataLogging(sqlConfig.EnableSensitiveDataLogging)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), sqlConfig.PoolSize);

            return services;
        }

        public static IServiceCollection AddMiddleware(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services
                .AddControllers(options =>
                {
                    options.Filters.Add(new ProducesAttribute("application/json"));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Dotnet Next Api",
                        Version = "v1",
                    });

                var filePath = Path.Combine(AppContext.BaseDirectory, "Api.xml");
                options.IncludeXmlComments(filePath);
            });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IMailProvider, SystemMailProvider>();
            services.AddSingleton<IJwtService, JwtService>();

            return services;
        }
    }
}
