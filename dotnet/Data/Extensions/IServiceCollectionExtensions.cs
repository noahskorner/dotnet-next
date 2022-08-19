﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Data.Configuration;
using Data.Entities.User;

namespace Data.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDatabase(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSqlServer(configuration);
            services.AddServices();

            return services;
        }

        public static IServiceCollection AddSqlServer(this IServiceCollection services, ConfigurationManager configuration)
        {
            var sqlConfig = configuration.GetSection(SqlServerConfiguration.SqlServer).Get<SqlServerConfiguration>();
            services.AddSingleton((sp) => sqlConfig);

            services
                .AddDbContextPool<ApiContext>(options => options
                    .UseSqlServer(sqlConfig.ConnectionString, x => x.EnableRetryOnFailure())
                    .EnableSensitiveDataLogging(sqlConfig.EnableSensitiveDataLogging)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), sqlConfig.PoolSize);

            return services;
        }

        public static IServiceCollection AddInMemoryDatabase(this IServiceCollection services)
        {
            services
                .AddDbContextPool<ApiContext>(options => options
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .EnableSensitiveDataLogging(true)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), 256);

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICreateUser, CreateUser>();
            services.AddScoped<IGetUserByEmail, GetUserByEmail>();

            return services;
        }
    }
}
