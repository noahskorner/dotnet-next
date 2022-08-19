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
    }
}
