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
using Api.Constants;
using Api.Controllers.Api.Auth.Login;
using Api.Controllers.Api.Auth;
using Api.Models;

namespace Test
{
    public class ControllerFixture : BaseFixture
    {
        protected HttpClient _sut;
        protected ApiContext _context;
        private IServiceProvider _serviceProvider;

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

                        _serviceProvider = services.BuildServiceProvider();
                        _context = _serviceProvider.GetRequiredService<ApiContext>();
                    });
                });

            _sut = application.CreateClient();
        }

        [TearDown]
        public void ControllerTearDown()
        {
            _context.Database.EnsureDeleted();
        }

        protected TService GetDep<TService>() where TService : class
        {
            return _serviceProvider.GetRequiredService<TService>();
        }

        protected async Task<UserDto> CreateUser(string email = "jon.snow@gmail.com", string password = "123456aB$", bool isEmailVerified = true)
        {
            var createUserRequest = new CreateUserRequest(email, password);
            var createUserResult = await _sut.PostAsJsonAsync($"{ApiConstants.ROUTE_PREFIX}/user", createUserRequest).AsCreated<UserDto>();
            var user = createUserResult.Data;

            if (isEmailVerified)
            {
                var userEntity = await _context.User.FindAsync(user.Id);
                var emailVerificationToken = userEntity?.EmailVerificationToken ?? throw new Exception();
                await _sut.PutAsync($"{ApiConstants.ROUTE_PREFIX}/user/{user.Id}/verify-email/{emailVerificationToken}", null);
            }

            return user;
        }

        protected async Task<HttpRequestMessage> CreateAndLoginUser(HttpMethod httpMethod, string requestUri)
        {
            var authUrl = $"{ApiConstants.ROUTE_PREFIX}/auth";
            var password = "123456aB$";
            var user = await CreateUser(password: password);
            var loginRequest = new LoginRequest(user.Email, password);
            var loginResponse = await _sut.PostAsJsonAsync(authUrl, loginRequest);
            var content = await loginResponse.Content.ReadAsStringAsync();
            var authResponse = await loginResponse.Content.ReadFromJsonAsync<Result<AuthResponse>>() ?? throw new ArgumentNullException();
            var refreshTokenCookieValue = loginResponse.Headers
                .Single(header => header.Key == "Set-Cookie" && header.Value.Where(x => x.Contains(ApiConstants.TOKEN_COOKIE_KEY)).Count() == 1)
                .Value
                .Single();
            var refreshToken = refreshTokenCookieValue.Split('=')[1].Split(';')[0];
            var request = new HttpRequestMessage(httpMethod, requestUri);
            request.Headers.Add("Cookie", $"{ApiConstants.TOKEN_COOKIE_KEY}={refreshToken};");
            request.Headers.Add("Authorization", $"Bearer {authResponse.Data.AccessToken}");

            return request;
        }
    }
}
