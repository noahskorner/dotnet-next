using Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class ApiContext : Context
    {
        public DbSet<User> User { get; set; }
        public ApiContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
