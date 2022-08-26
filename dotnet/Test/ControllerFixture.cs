using Data;
using Data.Extensions;
using Services.Providers.MailProvider;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Test.Extensions;
using Services.Features.Users;
using Api.Controllers.Api.Users.Create;
using System.Net.Http.Json;

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

        protected async Task<UserDto> CreateUser(string email =  "jon.snow@gmail.com", string password = "123456aB$")
        {
            var createUserRequest = new CreateUserRequest(email, password);
            var createUserResult = await _sut.PostAsJsonAsync("v1/user", createUserRequest).AsCreated<UserDto>();
            
            return createUserResult.Data;
        }
    }
}
