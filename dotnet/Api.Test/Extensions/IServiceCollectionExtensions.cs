using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Test.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void RemoveDep(this IServiceCollection services, Type type)
        {
            var descriptor = services.SingleOrDefault(x => x.ServiceType == type);

            if (descriptor != null) services.Remove(descriptor);
        }

        public static IServiceCollection AddInMemoryDatabase(this IServiceCollection services)
        {
            services
                .AddDbContextPool<ApiContext>(options => options
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .EnableSensitiveDataLogging(true)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), 256);;

            return services;
        }
    }
}
