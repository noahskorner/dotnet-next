using Data.Repositories.UserRoles;
using Data.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApiContext : Context
    {
        public DbSet<UserEntity> User { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }

        public ApiContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());
        }
    }
}
