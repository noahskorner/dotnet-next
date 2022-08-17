using Bogus;

namespace Api.Test
{
    public class BaseFixture
    {
        protected Faker _faker;

        [OneTimeSetUp]
        public void SetUp()
        {
            _faker = new Faker();
        }
    }
}
