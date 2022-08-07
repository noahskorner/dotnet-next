namespace Api.Configuration
{
    public class SqlServerConfiguration
    {
        public const string SqlServer = "SqlServer";
        public string ConnectionString { get; set; }
        public int PoolSize { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
    }
}
