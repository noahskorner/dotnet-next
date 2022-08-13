using Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Test
{
    public class ControllerFixture<TController> : BaseFixture where TController : ControllerBase
    {
        private static ConfigurationManager _cachedConfigurationManager;
        private static IServiceCollection _cachedServiceCollection;
        private static IServiceProvider _cachedServiceProvider;
        private static TController _cachedController;

        private ConfigurationManager ConfigurationManager
        {
            get
            {
                if (_cachedConfigurationManager != null) return _cachedConfigurationManager;

                var basePath = GetAppSettingsBasePath(); // Get the project directories appsettings files. This is weird but fuck it
                var configurationManager = new ConfigurationManager()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.Test.json");

                _cachedConfigurationManager = (ConfigurationManager)configurationManager;
                return _cachedConfigurationManager;
            }
        }
        private IServiceCollection ServiceCollection
        {
            get
            {
                if (_cachedServiceCollection != null) return _cachedServiceCollection;

                var services = new ServiceCollection()
                    .AddConfiguration(ConfigurationManager)
                    .AddCors(ConfigurationManager)
                    .AddInMemoryDatabase(ConfigurationManager)
                    .AddMiddleware()
                    .AddServices();

                _cachedServiceCollection = services;
                return _cachedServiceCollection;
            }
        }       

        protected TController _sut
        {
            get
            {
                if (_cachedController != null) return _cachedController;
                
                ServiceCollection.AddScoped(typeof(TController), typeof(TController));

                _cachedController = GetDep<TController>();
                return _cachedController;
            }
        }

        protected TService GetDep<TService>() where TService : class
        {
            var instance = ServiceProvider.GetService<TService>();
            if (instance == null) throw new ServiceNotRegisteredException(nameof(TService));

            return instance;
        }
        protected void RegisterDep<TService>(TService mockService) where TService : class
        {
            var existingService = ServiceCollection.FirstOrDefault(x => x.ServiceType == typeof(TService));
            if (existingService != null) ServiceCollection.Remove(existingService);

            ServiceCollection.AddScoped(sp => mockService);
        }

        private string GetAppSettingsBasePath()
        {
            var solutionDirectory = GetSolutionDirectory();
            var projectName = GetProjectName();

            return Path.Combine(solutionDirectory, projectName);
        }
        private string GetProjectName()
        {
            var type = GetType();
            if (type == null) throw new AppSettingsNotFoundException();

            if (type.Namespace == null) throw new AppSettingsNotFoundException();

            var splitNamespace = type.Namespace.Split(".");
            if (!splitNamespace.Any()) throw new AppSettingsNotFoundException();

            return splitNamespace[0];
        }
        private string GetSolutionDirectory()
        {
            var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            if (parentDirectory == null) throw new AppSettingsNotFoundException();

            var binDirectory = parentDirectory.Parent;
            if (binDirectory == null) throw new AppSettingsNotFoundException();

            var projectDirectory = binDirectory.Parent;
            if (projectDirectory == null) throw new AppSettingsNotFoundException();

            var solutionDirectory = projectDirectory.Parent;
            if (solutionDirectory == null) throw new AppSettingsNotFoundException();

            return solutionDirectory.FullName;
        }
    }

    public class AppSettingsNotFoundException : Exception
    {
        public AppSettingsNotFoundException() : base("Something is wrong. I am looking for appsettings.json in {solution}/{project}/appsettings.json")
        {
        }
    }
    public class ServiceNotRegisteredException : Exception
    {
        public ServiceNotRegisteredException(string serviceName) : base($"{serviceName} was not registered to the Dependency Injection container")
        {
        }
    }
}
