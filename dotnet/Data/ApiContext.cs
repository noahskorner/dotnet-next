using Data.Repositories.Users;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApiContext : Context
    {
        public DbSet<UserEntity> User { get; set; }

        public ApiContext(
            DbContextOptions options,
            IDateService dateService) : base(options, dateService)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
