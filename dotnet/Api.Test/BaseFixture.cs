using Bogus;

namespace Api.Test
{
    public class BaseFixture
    {
        protected readonly Faker _faker;

        public BaseFixture()
        {
            _faker = new Faker();
        }
    }
}
