using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services.AddServices();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateService, DateService>();

            return services;
        }
    }
}
