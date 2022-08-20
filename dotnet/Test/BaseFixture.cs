using Bogus;

namespace Test
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
