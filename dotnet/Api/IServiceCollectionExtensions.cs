using Api.Configuration;
using Api.Data;
using Api.Services.JwtService;
using Api.Services.MailService;
using Api.Services.PasswordService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Api
{
    public static class IServiceCollectionExtensions
    {
        public static void AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton((sp) => configuration.GetSection(CorsConfiguration.Cors).Get<CorsConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(SqlServerConfiguration.SqlServer).Get<SqlServerConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(SmtpConfiguration.Smtp).Get<SmtpConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(JwtConfiguration.Jwt).Get<JwtConfiguration>());
            services.AddSingleton((sp) => configuration.GetSection(FrontendConfiguration.Frontend).Get<FrontendConfiguration>());
        }

        public static void AddCors(this IServiceCollection services, CorsConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: config.PolicyName, policy =>
                {
                    policy.WithOrigins(config.AllowedOrigins);
                });
            });
        }

        public static void AddSqlServer(this IServiceCollection services, SqlServerConfiguration config)
        {
            services.AddDbContextPool<ApiContext>(options => options
                    .UseSqlServer(config.ConnectionString, x => x.EnableRetryOnFailure())
                    .EnableSensitiveDataLogging(config.EnableSensitiveDataLogging)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), config.PoolSize);
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddControllers();
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

            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IMailService, MailService>();
            services.AddSingleton<IJwtService, JwtService>();
        }
    }
}
