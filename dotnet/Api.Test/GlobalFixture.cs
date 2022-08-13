namespace Api.Test
{
    [SetUpFixture]
    public class GlobalFixture
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        }

        [OneTimeTearDown]
        public void TearDown()
        {

        }
    }
}
