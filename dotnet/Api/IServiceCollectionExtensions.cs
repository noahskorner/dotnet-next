using Api.Services.MailProvider;
using Api.Services.PasswordManager;
using MediatR;
using System.Reflection;

namespace Api
{
    public static class IServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddSingleton<IPasswordManager, PasswordManager>();
            services.AddSingleton<IMailProvider, MailProvider>();
        }
    }
}
