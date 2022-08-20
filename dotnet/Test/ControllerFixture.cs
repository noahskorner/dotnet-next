using Data;
using Data.Extensions;
using Domain.Providers.MailProvider;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Test.Extensions;

namespace Test
{
    public class ControllerFixture : BaseFixture
    {
        protected HttpClient _sut;
        protected ApiContext _context;

        [SetUp]
        public void ControllerSetUp()
        {
            var emailProviderMock = new Mock<IMailProvider>();
            emailProviderMock.Setup(x => x.SendMailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveDep(typeof(IMailProvider));
                        services.AddSingleton(sp => emailProviderMock.Object);

                        services.RemoveDep(typeof(ApiContext));
                        services.AddInMemoryDatabase();

                        var serviceProvider = services.BuildServiceProvider();
                        _context = serviceProvider.GetRequiredService<ApiContext>();
                    });
                });

            _sut = application.CreateClient();
        }

        [TearDown]
        public void ControllerTearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
