using Data.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApiContext : Context
    {
        public DbSet<UserEntity> User { get; set; }

        public ApiContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
