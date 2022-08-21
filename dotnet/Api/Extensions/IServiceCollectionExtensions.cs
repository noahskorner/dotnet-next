using Api.Configuration;
using Api.Extensions;
using Api.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;
using System.Text.Json;

namespace Api.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApi(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddCors(configuration);
            services.AddMiddleware();
            services.AddLocalization();
            services.AddSingleton<LocalizationMiddleware>();
            services.AddDistributedMemoryCache();
            services.AddSwagger();
            services.AddServices();

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

        public static IServiceCollection AddMiddleware(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
            services.AddEndpointsApiExplorer();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
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

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();

            return services;
        }
    }
}
