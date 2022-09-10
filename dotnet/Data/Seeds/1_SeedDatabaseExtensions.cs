namespace Data.Seeds
{
    public static class SeedDatabaseExtensions
    {
        public static void SeedDatabase(this ApiContext context)
        {
           context.SeedRoles();
        }
    }
}
