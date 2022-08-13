namespace Api.Extensions
{
    public static class IWebHostEnvironmentExtensions
    {
        public static bool IsTest(this IWebHostEnvironment host)
        {
            return host.IsEnvironment("Test");
        }
    }
}
