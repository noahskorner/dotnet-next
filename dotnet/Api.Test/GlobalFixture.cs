namespace Api.Test
{
    [SetUpFixture]
    public class GlobalFixture
    {
        [OneTimeSetUp]
        public void GlobalOneTimeSetup()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        }

        [OneTimeTearDown]
        public void GlobalOneTimeTearDown()
        {

        }
    }
}
