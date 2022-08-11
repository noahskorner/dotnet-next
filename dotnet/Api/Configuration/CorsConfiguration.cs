namespace Api.Configuration
{
    public class CorsConfiguration
    {
        public const string Cors = "Cors";
        public string PolicyName { get; set; }
        public string[] AllowedOrigins { get; set; }
        public string[] AllowedMethods { get; set; }
    }
}
