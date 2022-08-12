using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Test
{
    public class ControllerFixture
    {
        protected readonly HttpClient _sut;
        protected readonly Faker _faker;

        public ControllerFixture()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            _sut = webApplicationFactory.CreateClient();
            _faker = new Faker();
        }
    }
}
