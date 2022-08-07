using Api.Configuration;
using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("appsettings.json");

            builder.Services.AddSingleton((sp) => builder.Configuration.GetSection(CorsConfiguration.Cors).Get<CorsConfiguration>());
            builder.Services.AddSingleton((sp) => builder.Configuration.GetSection(SqlServerConfiguration.SqlServer).Get<SqlServerConfiguration>());
            builder.Services.AddSingleton((sp) => builder.Configuration.GetSection(SmtpConfiguration.Smtp).Get<SmtpConfiguration>());
        }

        public static void AddCors(this WebApplicationBuilder builder)
        {
            var config = builder.Configuration.GetSection(CorsConfiguration.Cors).Get<CorsConfiguration>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: config.PolicyName, policy =>
                {
                    policy.WithOrigins(config.AllowedOrigins);
                });
            });
        }

        public static void AddSqlServer(this WebApplicationBuilder builder)
        {
            var config = builder.Configuration.GetSection(SqlServerConfiguration.SqlServer).Get<SqlServerConfiguration>();

            builder.Services.AddDbContextPool<ApiContext>(options => options
                .UseSqlServer(config.ConnectionString, x => x.EnableRetryOnFailure())
                .EnableSensitiveDataLogging(config.EnableSensitiveDataLogging)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), config.PoolSize);
        }
    }
}
